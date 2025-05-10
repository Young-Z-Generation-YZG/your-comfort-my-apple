using Google.Protobuf.WellKnownTypes;
using MassTransit;
using Microsoft.AspNetCore.Http;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.Abstractions.Providers.Momo;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket.Extensions;
using YGZ.Basket.Domain.Core.Enums;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Discount.Grpc.Protos;
using static MassTransit.ValidationResultExtensions;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

public sealed record CheckoutBasketCommandHandler : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishIntegrationEvent;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserContext _userContext;
    private readonly IVnpayProvider _vnpayProvider;
    private readonly IMomoProvider _momoProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutBasketCommandHandler(IBasketRepository basketRepository,
                                        IPublishEndpoint publishEndpoint,
                                        IUserContext userContext,
                                        DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
                                        IVnpayProvider vnpayProvider,
                                        IHttpContextAccessor httpContextAccessor,
                                        IMomoProvider momoProvider)
    {
        _basketRepository = basketRepository;
        _publishIntegrationEvent = publishEndpoint;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
        _vnpayProvider = vnpayProvider;
        _httpContextAccessor = httpContextAccessor;
        _momoProvider = momoProvider;
    }

    public async Task<Result<CheckoutBasketResponse>> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.Response is null || !result.Response.CartItems.Any())
        {
            return Errors.Basket.BasketEmpty;
        }

        decimal discountAmount = 0;
        decimal subTotal = result.Response.CartItems.Sum(c => c.ProductUnitPrice * c.Quantity);

        if (!result.Response!.CartItems.Any())
        {
            return Errors.Basket.BasketEmpty;
        }

        if (!string.IsNullOrEmpty(request.DiscountCode))
        {
            result.Response!.CartItems = await HandlerPromotionCoupon(request.DiscountCode, result.Response!.CartItems);
        }

        await HandleCheckPromotion(result.Response!, cancellationToken);

        discountAmount = (decimal)(result.Response.CartItems.Sum(c => c.Quantity * c.ProductUnitPrice) - result.Response.CartItems.Sum(c => c.SubTotalAmount))!;

        decimal totalAmount = subTotal - discountAmount;

        var orderId = Guid.NewGuid();

        var eventMessage = request.ToBasketCheckoutIntegrationEvent(orderId: orderId, customerId: userId,
                                                                    customerEmail: userEmail,
                                                                    cartItems: result.Response.CartItems,
                                                                    subTotalAmount: subTotal,
                                                                    discountAmount: discountAmount,
                                                                    totalAmount: totalAmount);

        await _publishIntegrationEvent.Publish(eventMessage, cancellationToken);

        switch (request.PaymentMethod)
        {
            case nameof(PaymentMethod.VNPAY):
                var model = new VnpayInformationModel()
                {
                    OrderType = "VNPAY_CHECKOUT",
                    OrderDescription = $"ORDER_ID={orderId}",
                    Amount = result.Response.TotalAmount * 25000,
                    Name = request.ContactName,
                };

                var paymentUrl = _vnpayProvider.CreatePaymentUrl(model, _httpContextAccessor.HttpContext!);
                if (string.IsNullOrEmpty(paymentUrl))
                {
                    return Errors.Payment.VnpayPaymentUrlInvalid;
                }

                return new CheckoutBasketResponse() { PaymentRedirectUrl = paymentUrl };
            case nameof(PaymentMethod.MOMO):
                var momoPaymentUrl = await _momoProvider.CreatePaymentUrlAsync(new MomoInformationModel()
                {
                    FullName = $"{request.ContactName}",
                    OrderId = $"ORDER_ID={orderId}",
                    OrderInfo = "MOMO_CHECKOUT",
                    Amount = (double)10 * 25000,
                });

                if (momoPaymentUrl?.ErrorCode != 0)
                {
                    return Errors.Payment.MomoPaymentUrlInvalid;
                }

                return new CheckoutBasketResponse() { PaymentRedirectUrl = momoPaymentUrl!.PayUrl };
            case nameof(PaymentMethod.COD):
                return new CheckoutBasketResponse() { OrderDetailsRedirectUrl = $"/account/orders/{orderId}" };
            default:
                return Errors.Payment.Invalid;
        }
    }

    private async Task<List<ShoppingCartItem>> HandlerPromotionCoupon(string couponCode, List<ShoppingCartItem> cartItems)
    {
        var coupon = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = couponCode });

        if (coupon.PromotionCoupon is null)
        {
            return cartItems;
        }

        var dateTimeNow = Timestamp.FromDateTime(DateTime.UtcNow);

        if (coupon.PromotionCoupon.PromotionCouponDiscountState != DiscountStateEnum.Active)
        {
            return cartItems;
        }

        if (!(coupon.PromotionCoupon.PromotionCouponValidFrom < dateTimeNow && dateTimeNow < coupon.PromotionCoupon.PromotionCouponValidTo))
        {
            return cartItems;
        }

        for (int i = 0; i < cartItems.Count; i++)
        {
            var item = cartItems[i];

            if (item.Promotion is not null)
            {
                continue;
            }
            else
            {
                ShoppingCartItem updatedItem;

                switch (coupon.PromotionCoupon.PromotionCouponPromotionEventType)
                {
                    case PromotionEventTypeEnum.PromotionCoupon:
                        updatedItem = HandleCouponPromotion(item, coupon);

                        cartItems[i] = updatedItem;
                        break;
                }
            }
        }

        return cartItems;
    }

    private ShoppingCartItem HandleCouponPromotion(ShoppingCartItem item, CouponResponse couponDiscount)
    {
        decimal promotionDiscountUnitPrice = 0;
        decimal promotionFinalPrice = 0;
        int promotionAppliedProductCount = 0;
        int remainAvailableQuantityCoupon = 0;

        decimal subTotalAmount = item.Quantity * item.ProductUnitPrice;
        promotionFinalPrice = subTotalAmount;

        if (couponDiscount is not null)
        {
            remainAvailableQuantityCoupon = item.Quantity - (int)couponDiscount.PromotionCoupon.PromotionCouponAvailableQuantity!;

            if (remainAvailableQuantityCoupon < 0)
            {
                if ((int)couponDiscount.PromotionCoupon.PromotionCouponDiscountType == DiscountType.PERCENTAGE.Value)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!);
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    promotionAppliedProductCount = item.Quantity;
                }
                else if ((int)couponDiscount.PromotionCoupon.PromotionCouponDiscountType == DiscountType.FIXED.Value)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!;
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    promotionAppliedProductCount = item.Quantity;
                }
            }
            else
            {
                promotionDiscountUnitPrice = item.ProductUnitPrice;
                promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                promotionAppliedProductCount = item.Quantity - remainAvailableQuantityCoupon;
            }

            subTotalAmount = promotionFinalPrice;
        }
        else
        {
            item.Promotion = null;

            return item;
        }

        var promotion = Promotion.Create(promotionIdOrCode: couponDiscount.PromotionCoupon.PromotionCouponId,
                                         promotionEventType: PromotionEvent.FromValue((int)couponDiscount.PromotionCoupon.PromotionCouponPromotionEventType).Name,
                                         promotionTitle: couponDiscount.PromotionCoupon.PromotionCouponTitle,
                                         promotionDiscountType: DiscountType.FromValue((int)couponDiscount.PromotionCoupon.PromotionCouponDiscountType).Name,
                                         promotionDiscountValue: (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!,
                                         promotionDiscountUnitPrice: promotionDiscountUnitPrice,
                                         promotionAppliedProductCount: promotionAppliedProductCount,
                                         promotionFinalPrice: promotionFinalPrice);

        item.Promotion = promotion;

        item.SubTotalAmount = subTotalAmount;

        return item;
    }

    private async Task<ShoppingCart> HandleCheckPromotion(ShoppingCart shoppingCart, CancellationToken cancellation)
    {
        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            return shoppingCart;
        }

        ListPromtionEventResponse promotionEvent = await HandleCheckValidPromotionEvent();

        for (int i = 0; i < shoppingCart.CartItems.Count; i++)
        {
            var item = shoppingCart.CartItems[i];

            if (item.Promotion is null)
            {
                continue;
            }
            else
            {
                ShoppingCartItem updatedItem;

                switch (item.Promotion.PromotionEventType)
                {
                    case nameof(PromotionEvent.PROMOTION_ITEM):
                        updatedItem = HandleItemPromotion(item).Result;
                        shoppingCart.CartItems[i] = updatedItem;
                        break;
                    case nameof(PromotionEvent.PROMOTION_EVENT):
                        if (promotionEvent is not null)
                        {
                            updatedItem = HandleEventPromotion(item, promotionEvent);
                            shoppingCart.CartItems[i] = updatedItem;
                        }
                        else
                        {
                            shoppingCart.CartItems[i].Promotion = null;
                            shoppingCart.CartItems[i].SubTotalAmount = shoppingCart.CartItems[i].ProductUnitPrice * shoppingCart.CartItems[i].Quantity;

                            await _basketRepository.StoreBasketAsync(shoppingCart, cancellation);
                        }
                        break;
                }
            }
        }

        return shoppingCart;
    }

    private async Task<ListPromtionEventResponse> HandleCheckValidPromotionEvent()
    {
        var result = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest { });

        if (result is null || !result.PromotionEvents.Any())
        {
            return null;
        }
        var dateTimeNow = Timestamp.FromDateTime(DateTime.UtcNow);

        var promotionEvent = result.PromotionEvents
            .Where(x => x.PromotionEvent.PromotionEventState == DiscountStateEnum.Active)
            .Where(x => x.PromotionEvent.PromotionEventValidFrom <= dateTimeNow && dateTimeNow <= x.PromotionEvent.PromotionEventValidTo)
            .FirstOrDefault();

        if (promotionEvent is null)
        {
            return null;
        }

        return promotionEvent;
    }

    private async Task<ShoppingCartItem> HandleItemPromotion(ShoppingCartItem item)
    {
        decimal promotionDiscountUnitPrice = -1;
        decimal promotionFinalPrice = 0;
        decimal discountValue = 0;
        decimal subTotalAmount = item.Quantity * item.ProductUnitPrice;
        int promotionAppliedProductCount = 0;
        DiscountTypeEnum discountType = DiscountTypeEnum.Percentage;

        var promotionItem = await _discountProtoServiceClient.GetPromotionItemByIdAsync(new GetPromotionItemByIdRequest
        {
            PromotionId = item.Promotion!.PromotionIdOrCode
        });

        var dateTimeNow = Timestamp.FromDateTime(DateTime.UtcNow);

        if (promotionItem is null)
        {
            item.Promotion = null;

            return item;
        }

        if (promotionItem.PromotionItemDiscountState != DiscountStateEnum.Active)
        {
            item.Promotion = null;

            return item;
        }

        if (promotionItem.PromotionItemEndDiscountType == EndDiscountEnum.ByEndDate && promotionItem.PromotionItemValidFrom <= dateTimeNow && dateTimeNow <= promotionItem.PromotionItemValidTo)
        {
            discountType = promotionItem.PromotionItemDiscountType;
            discountValue = (decimal)promotionItem.PromotionItemDiscountValue!;

            if (discountType == DiscountTypeEnum.Percentage)
            {
                promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionItem.PromotionItemDiscountValue!);
                promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                discountType = DiscountTypeEnum.Percentage;
            }
            else
            {
                promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)promotionItem.PromotionItemDiscountValue!;
                promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                discountType = DiscountTypeEnum.Fixed;
            }

            promotionAppliedProductCount = item.Quantity;
            item.Promotion!.PromotionTitle = promotionItem.PromotionItemTitle;

        }
        else if (promotionItem.PromotionItemEndDiscountType == EndDiscountEnum.ByQuantity && promotionItem.PromotionItemAvailableQuantity > 0)
        {
            item.Promotion = null;
            return item;
        }
        else
        {
            item.Promotion = null;
            return item;
        }

        item.Promotion!.PromotionDiscountType = discountType.ToString().ToUpper();
        item.Promotion.PromotionDiscountValue = discountValue;
        item.Promotion!.PromotionDiscountUnitPrice = promotionDiscountUnitPrice;
        item.Promotion.PromotionAppliedProductCount = promotionAppliedProductCount;
        item.Promotion.PromotionFinalPrice = promotionFinalPrice;

        return item;
    }

    private ShoppingCartItem HandleEventPromotion(ShoppingCartItem item, ListPromtionEventResponse promotionEvent)
    {
        decimal promotionDiscountUnitPrice = -1;
        decimal promotionFinalPrice = 0;
        int promotionAppliedProductCount = 0;
        DiscountTypeEnum discountType = DiscountTypeEnum.Percentage;
        decimal discountValue = 0;

        decimal subTotalAmount = item.Quantity * item.ProductUnitPrice;

        if (promotionEvent is not null)
        {
            List<PromotionProductModel> promotionProducts = new List<PromotionProductModel>();
            List<PromotionCategoryModel> promotionCategories = new List<PromotionCategoryModel>();

            promotionProducts = promotionEvent.PromotionProducts.ToList();
            promotionCategories = promotionEvent.PromotionCategories.ToList();

            var promotionProduct = promotionProducts.FirstOrDefault(pp => pp.PromotionProductId == item.ProductSlug);
            var promotionCategory = promotionCategories.FirstOrDefault(pc => pc.PromotionCategoryId == item.CategoryId);



            if (promotionProduct is not null && promotionProduct.PromotionProductSlug == item.ProductSlug)
            {
                discountType = promotionProduct.PromotionProductDiscountType;

                if (discountType == DiscountTypeEnum.Percentage)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionProduct.PromotionProductDiscountValue!);
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    discountType = DiscountTypeEnum.Percentage;
                    discountValue = (decimal)promotionProduct.PromotionProductDiscountValue!;
                }
                else
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)promotionProduct.PromotionProductDiscountValue!;
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    discountType = DiscountTypeEnum.Fixed;
                    discountValue = (decimal)promotionProduct.PromotionProductDiscountValue!;
                }

                promotionAppliedProductCount = item.Quantity;
            }

            if (promotionCategory is not null)
            {
                discountType = promotionCategory.PromotionCategoryDiscountType;

                if (discountType == DiscountTypeEnum.Percentage)
                {
                    decimal categoryDiscountPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionCategory.PromotionCategoryDiscountValue!);

                    if (promotionDiscountUnitPrice == -1 || categoryDiscountPrice < promotionDiscountUnitPrice)
                    {
                        promotionDiscountUnitPrice = categoryDiscountPrice;
                        promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                        discountType = DiscountTypeEnum.Percentage;
                        discountValue = (decimal)promotionCategory.PromotionCategoryDiscountValue!;
                    }
                }

                promotionAppliedProductCount = item.Quantity;
            }

            if (promotionDiscountUnitPrice == -1)
            {
                item.Promotion = null;

                return item;
            }

            subTotalAmount = promotionFinalPrice;
            item.Promotion!.PromotionTitle = promotionEvent.PromotionEvent.PromotionEventTitle;
        }
        else
        {
            item.Promotion = null;

            return item;
        }

        item.Promotion!.PromotionDiscountType = discountType.ToString().ToUpper();
        item.Promotion.PromotionDiscountValue = discountValue;
        item.Promotion!.PromotionDiscountUnitPrice = promotionDiscountUnitPrice;
        item.Promotion.PromotionAppliedProductCount = promotionAppliedProductCount;
        item.Promotion.PromotionFinalPrice = promotionFinalPrice;
            
        item.SubTotalAmount = subTotalAmount;

        return item;
    }

    private decimal CaculateDiscountUnit(DiscountType discountType, decimal promotionCouponDiscountValue, decimal productUnitPrice)
    {
        decimal discountUnit = 0;

        switch (discountType.Name)
        {
            case nameof(DiscountType.PERCENTAGE):
                discountUnit = productUnitPrice - (productUnitPrice * promotionCouponDiscountValue);
                break;
            case nameof(DiscountType.FIXED):
                discountUnit = productUnitPrice - promotionCouponDiscountValue;
                break;
        }

        return discountUnit;
    }
}
