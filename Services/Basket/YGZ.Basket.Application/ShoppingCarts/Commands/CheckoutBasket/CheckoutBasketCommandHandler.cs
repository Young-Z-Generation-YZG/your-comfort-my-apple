using MassTransit;
using Microsoft.AspNetCore.Http;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.Abstractions.Providers.Momo;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket.Extensions;
using YGZ.Basket.Domain.Core.Enums;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Discount.Grpc.Protos;

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
        //var userEmail = _userContext.GetUserEmail();
        //var userId = _userContext.GetUserId();

        var basket = await _basketRepository.GetBasketAsync("lov3rinve146@gmail.com", cancellationToken);

        if (basket.Response is null || !basket.Response.CartItems.Any())
        {
            return Errors.Basket.BasketEmpty;
        }

        var discountAmount = 0;
        var subTotal = basket.Response.CartItems.Sum(c => c.SubTotalAmount);
        var total = basket.Response.TotalAmount;

        if (!string.IsNullOrEmpty(request.DiscountCode) && basket.Response!.CartItems.Any())
        {
            var discount = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = request.DiscountCode });

            if (discount is null)
            {
                return Errors.Discount.PromotionCouponNotFound;
            }

            var discountType = DiscountType.FromValue((int)discount.PromotionCoupon.PromotionCouponDiscountType);

            for (var i = 0; i < basket.Response.CartItems.Count; i++)
            {
                var cartItem = basket.Response.CartItems[i];

                if (cartItem.Promotion is not null) continue;

                var discountUnit = CaculateDiscountUnit(discountType: discountType,
                                                        promotionCouponDiscountValue: (decimal)discount.PromotionCoupon.PromotionCouponDiscountValue!,
                                                        productUnitPrice: cartItem.ProductUnitPrice);

                var appliedCount = cartItem.Quantity;


                var promotion = Promotion.Create(promotionIdOrCode: discount.PromotionCoupon.PromotionCouponCode,
                                                 promotionEventType: PromotionEvent.PROMOTION_COUPON.Name,
                                                 promotionTitle: discount.PromotionCoupon.PromotionCouponTitle,
                                                 promotionDiscountType: discountType.Name,
                                                 promotionDiscountValue: (decimal)discount.PromotionCoupon.PromotionCouponDiscountValue,
                                                 promotionDiscountUnitPrice: discountUnit,
                                                 promotionAppliedProductCount: appliedCount,
                                                 promotionFinalPrice: discountUnit * appliedCount);

                basket.Response.CartItems[i].SubTotalAmount = discountUnit * cartItem.Quantity;
                basket.Response.CartItems[i].Promotion = promotion;
            }
        }

        var orderId = Guid.NewGuid();

        subTotal = basket.Response.CartItems.Sum(c => c.SubTotalAmount);

        var eventMessage = request.ToBasketCheckoutIntegrationEvent(orderId: orderId, customerId: "ed04b044-86de-475f-9122-d9807897f969",
                                                                    customerEmail: "lov3rinve146@gmail.com",
                                                                    cartItems: basket.Response.CartItems,
                                                                    subTotalAmount: (decimal)subTotal!,
                                                                    discountAmount: discountAmount,
                                                                    totalAmount: total);

        await _publishIntegrationEvent.Publish(eventMessage, cancellationToken);

        //var promotionFinalPrice = discountUnit * appliedCount;

        switch (request.PaymentMethod)
        {
            case nameof(PaymentMethod.VNPAY):
                var model = new VnpayInformationModel()
                {
                    OrderType = "VNPAY_CHECKOUT",
                    OrderDescription = $"ORDER_ID={orderId}",
                    Amount = basket.Response.TotalAmount * 25000,
                    Name = request.ShippingAddress.ContactName,
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
                    FullName = $"{request.ShippingAddress.ContactName}",
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
