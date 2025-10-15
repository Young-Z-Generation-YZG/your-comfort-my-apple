﻿using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Http;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.Abstractions.Providers.Momo;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
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
    private readonly IUserRequestContext _userContext;
    private readonly IVnpayProvider _vnpayProvider;
    private readonly IMomoProvider _momoProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutBasketHandler(IBasketRepository basketRepository,
                                        IPublishEndpoint publishEndpoint,
                                        IUserRequestContext userContext,
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

        if (shoppingCart.CheckHasEventItems())
        {
            checkoutShoppingCart = shoppingCart.FilterEventItems();
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
                        return Errors.Discount.PromotionCouponNotFound;
                    }
                }

                if (coupon is not null && coupon.DiscountValue > 0)
                {
                    var quantity = coupon.AvailableQuantity;
                    var discountValue = (decimal)coupon.DiscountValue.Value;

                    foreach (var item in checkoutShoppingCart.CartItems)
                    {
                        if (item.IsSelected == true)
                        {
                            PromotionCoupon promotionCoupon = PromotionCoupon.Create(
                                couponId: coupon.Id, 
                                productUnitPrice: item.UnitPrice, 
                                discountType: ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString()), 
                                discountValue: discountValue, 
                                discountAmount: discountValue
                            );

                            item.ApplyCoupon(promotionCoupon);

                            quantity--;
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
                TotalAmount = checkoutShoppingCart.TotalAmount
            }
        };



        await _publishIntegrationEvent.Publish(integrationEventMessage, cancellationToken);

        //await _basketRepository.DeleteBasketAsync(userEmail, cancellationToken);

        switch (request.PaymentMethod)
        {
            case nameof(EPaymentMethod.VNPAY):
            var model = new VnpayInformationModel()
            {
                OrderType = "VNPAY_CHECKOUT",
                OrderDescription = $"ORDER_ID={orderId}",
                Amount = result.Response.TotalAmount * 25000,
                Name = request.ShippingAddress.ContactName,
            };

            var paymentUrl = _vnpayProvider.CreatePaymentUrl(model, _httpContextAccessor.HttpContext!);

            if (string.IsNullOrEmpty(paymentUrl))
            {
                return Errors.Payment.VnpayPaymentUrlInvalid;
            }

            return new CheckoutBasketResponse() { PaymentRedirectUrl = paymentUrl };

            case nameof(EPaymentMethod.MOMO):
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
            case nameof(EPaymentMethod.COD):
            return new CheckoutBasketResponse() { OrderDetailsRedirectUrl = $"/account/orders/{orderId}" };
            default:
            return Errors.Payment.Invalid;
        }
    }
}
