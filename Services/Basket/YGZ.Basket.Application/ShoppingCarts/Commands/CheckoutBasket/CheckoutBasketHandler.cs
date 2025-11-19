using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Http;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.Abstractions.Providers.Momo;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasket;

public sealed record CheckoutBasketHandler : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishIntegrationEvent;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserHttpContext _userContext;
    private readonly IVnpayProvider _vnpayProvider;
    private readonly IMomoProvider _momoProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutBasketHandler(IBasketRepository basketRepository,
                                 IPublishEndpoint publishEndpoint,
                                 IUserHttpContext userContext,
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

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            return Errors.Basket.BasketEmpty;
        }

        if (shoppingCart.CartItems.All(ci => ci.IsSelected == false))
        {
            return Errors.Basket.NotSelectedItems;
        }


        ShoppingCart checkoutShoppingCart;

        // Variables to store cart-level promotion data
        string? cartPromotionId = null;
        string? cartPromotionType = null;
        string? cartDiscountType = null;
        decimal? cartDiscountValue = null;
        decimal? cartDiscountAmount = null;

        if (shoppingCart.CheckHasEventItems())
        {
            checkoutShoppingCart = shoppingCart.FilterEventItems();

            cartPromotionId = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.EventItemId;
            cartPromotionType = EPromotionType.EVENT.Name;
            cartDiscountType = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.DiscountType;
            cartDiscountValue = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.DiscountValue;
            cartDiscountAmount = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.DiscountAmount;
        }
        else
        {
            checkoutShoppingCart = shoppingCart.FilterOutEventItemsAndSelected();

            // get coupon details if coupon code is provided
            if (!string.IsNullOrEmpty(request.DiscountCode))
            {
                var grpcRequest = new GetCouponByCodeRequest
                {
                    CouponCode = request.DiscountCode
                };

                CouponModel? coupon = null;

                try
                {
                    coupon = await _discountProtoServiceClient.GetCouponByCodeGrpcAsync(grpcRequest, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    if (ex.StatusCode == StatusCode.NotFound)
                    {
                        return Errors.Discount.CouponNotFound;
                    }
                }

                if (coupon is not null && coupon.DiscountValue > 0 && coupon.AvailableQuantity > 0)
                {
                    // Get only selected items
                    var selectedItems = checkoutShoppingCart.CartItems
                        .Where(item => item.IsSelected == true)
                        .ToList();

                    if (selectedItems.Any())
                    {
                        // Step 1: Calculate total cart value for selected items
                        decimal totalCartValue = selectedItems.Sum(item => item.UnitPrice * item.Quantity);

                        // Step 2: Calculate discount amount
                        var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString());
                        decimal calculatedDiscount = 0;

                        if (discountType == EDiscountType.PERCENTAGE)
                        {
                            // For percentage: discount = totalCartValue * (discountValue)
                            // Note: discountValue should already be in decimal form (e.g., 0.1 for 10%)
                            calculatedDiscount = totalCartValue * ((decimal)(coupon.DiscountValue ?? 0) / 100m);
                        }
                        else if (discountType == EDiscountType.FIXED_AMOUNT)
                        {
                            calculatedDiscount = (decimal)(coupon.DiscountValue ?? 0);
                        }

                        // Step 3: Apply maximum discount cap
                        decimal actualTotalDiscount = coupon.MaxDiscountAmount.HasValue && coupon.MaxDiscountAmount > 0
                            ? Math.Min(calculatedDiscount, (decimal)coupon.MaxDiscountAmount)
                            : calculatedDiscount;

                        // Store cart-level promotion data for integration event
                        cartPromotionId = coupon.Id;
                        cartPromotionType = EPromotionType.COUPON.Name;
                        cartDiscountType = discountType.Name;
                        cartDiscountValue = (decimal)(coupon.DiscountValue ?? 0);
                        cartDiscountAmount = actualTotalDiscount;

                        // Step 4: Distribute discount proportionally to each selected item
                        foreach (var item in selectedItems)
                        {
                            decimal itemSubtotal = item.UnitPrice * item.Quantity;
                            decimal itemProportion = totalCartValue > 0 ? itemSubtotal / totalCartValue : 0;
                            decimal itemTotalDiscount = actualTotalDiscount * itemProportion;

                            // Calculate discount per unit
                            decimal discountPerUnit = item.Quantity > 0 ? itemTotalDiscount / item.Quantity : 0;

                            // Calculate the effective discount value for this item
                            // This represents what percentage or amount was actually applied per unit
                            decimal effectiveDiscountValue = item.UnitPrice > 0
                                ? discountPerUnit / item.UnitPrice
                                : 0;

                            PromotionCoupon promotionCoupon = PromotionCoupon.Create(
                                couponId: coupon.Id,
                                productUnitPrice: item.UnitPrice,
                                discountType: discountType,
                                discountValue: effectiveDiscountValue, // Effective discount rate per unit
                                discountAmount: discountPerUnit // Actual discount amount per unit
                            );

                            item.ApplyCoupon(promotionCoupon);
                        }
                    }
                }
            }
        }


        var orderId = Guid.NewGuid();

        var orderItems = checkoutShoppingCart.CartItems
            .Select(x => x.ToCheckoutItemIntegrationEvent())
            .ToList();

        var integrationEventMessage = new BasketCheckoutIntegrationEvent
        {
            OrderId = orderId,
            CustomerId = userId,
            CustomerEmail = userEmail,
            CustomerPublicKey = null,
            Tx = null,
            ContactName = request.ShippingAddress.ContactName,
            ContactPhoneNumber = request.ShippingAddress.ContactPhoneNumber,
            AddressLine = request.ShippingAddress.AddressLine,
            District = request.ShippingAddress.District,
            Province = request.ShippingAddress.Province,
            Country = request.ShippingAddress.Country,
            PaymentMethod = request.PaymentMethod,
            Cart = new CartCommand
            {
                OrderItems = orderItems,
                PromotionId = cartPromotionId,
                PromotionType = cartPromotionType,
                DiscountType = cartDiscountType,
                DiscountValue = cartDiscountValue,
                DiscountAmount = cartDiscountAmount,
                TotalAmount = checkoutShoppingCart.TotalAmount
            }
        };



        await _publishIntegrationEvent.Publish(integrationEventMessage, cancellationToken);

        await _basketRepository.DeleteSelectedItemsBasketAsync(userEmail, cancellationToken);

        switch (request.PaymentMethod)
        {
            case nameof(EPaymentMethod.VNPAY):
            var model = new VnpayInformationModel()
            {
                OrderType = "VNPAY_CHECKOUT",
                OrderDescription = $"ORDER_ID={orderId}",
                Amount = checkoutShoppingCart.TotalAmount * 25000,
                Name = request.ShippingAddress.ContactName,
            };

            var paymentUrl = _vnpayProvider.CreatePaymentUrl(model, _httpContextAccessor.HttpContext!);

            if (string.IsNullOrEmpty(paymentUrl))
            {
                return Errors.Payment.VnpayPaymentUrlInvalid;
            }

            return new CheckoutBasketResponse()
            {
                OrderId = orderId.ToString(),
                CartItems = checkoutShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                PaymentRedirectUrl = paymentUrl
            };

            case nameof(EPaymentMethod.MOMO):
            var momoPaymentUrl = await _momoProvider.CreatePaymentUrlAsync(new MomoInformationModel()
            {
                FullName = $"{request.ShippingAddress.ContactName}",
                OrderId = $"ORDER_ID={orderId}",
                OrderInfo = "MOMO_CHECKOUT",
                Amount = (double)checkoutShoppingCart.TotalAmount * 25000,
            });

            if (momoPaymentUrl?.ErrorCode != 0)
            {
                return Errors.Payment.MomoPaymentUrlInvalid;
            }

            return new CheckoutBasketResponse()
            {
                OrderId = orderId.ToString(),
                CartItems = checkoutShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                PaymentRedirectUrl = momoPaymentUrl!.PayUrl
            };
            case nameof(EPaymentMethod.COD):
            return new CheckoutBasketResponse()
            {
                OrderId = orderId.ToString(),
                CartItems = checkoutShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                OrderDetailsRedirectUrl = $"/account/orders/{orderId}"
            };
            default:
            return Errors.Payment.Invalid;
        }
    }
}
