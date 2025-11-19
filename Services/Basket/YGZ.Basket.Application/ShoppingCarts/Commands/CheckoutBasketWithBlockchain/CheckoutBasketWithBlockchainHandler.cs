using Grpc.Core;
using MassTransit;
using YGZ.Basket.Application.Abstractions.Data;
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

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasketWithBlockchain;

public class CheckoutBasketWithBlockchainHandler : ICommandHandler<CheckoutBasketWithBlockchainCommand, CheckoutBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishIntegrationEvent;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserHttpContext _userContext;
    private readonly ITenantHttpContext _tenantContext;

    public CheckoutBasketWithBlockchainHandler(IBasketRepository basketRepository,
                                               IPublishEndpoint publishEndpoint,
                                               IUserHttpContext userContext,
                                               ITenantHttpContext tenantContext,
                                               DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _publishIntegrationEvent = publishEndpoint;
        _userContext = userContext;
        _tenantContext = tenantContext;
        _discountProtoServiceClient = discountProtoServiceClient;
    }


    public async Task<Result<CheckoutBasketResponse>> Handle(CheckoutBasketWithBlockchainCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();
        var tenantId = _tenantContext.GetTenantId();
        var branchId = _tenantContext.GetBranchId();

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

            cartPromotionId = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.PromotionId;
            cartPromotionType = EPromotionType.EVENT_ITEM.Name;
            cartDiscountType = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.DiscountType;
            cartDiscountValue = checkoutShoppingCart.CartItems.First().Promotion?.PromotionEvent?.DiscountValue;
            // Calculate total discount amount from cart items for event promotions
            cartDiscountAmount = checkoutShoppingCart.CartItems
                .Where(item => item.Promotion?.PromotionEvent != null)
                .Sum(item => item.DiscountAmount ?? 0);
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
                        // Convert CartItems to EfficientCart format
                        var efficientCartItems = selectedItems.Select((item, index) => new EfficientCartItem
                        {
                            UniqueString = $"item_{index}_{item.GetHashCode()}",
                            OriginalPrice = item.UnitPrice,
                            Quantity = item.Quantity,
                            PromotionId = item.Promotion?.PromotionCoupon?.PromotionId,
                            PromotionType = item.Promotion?.PromotionCoupon != null ? EPromotionType.COUPON.Name : null,
                            DiscountType = null,
                            DiscountValue = null,
                            DiscountAmount = null
                        }).ToList();

                        var beforeCart = new EfficientCart
                        {
                            CartItems = efficientCartItems,
                            PromotionId = coupon.Id,
                            PromotionType = EPromotionType.COUPON.Name,
                            DiscountType = null,
                            DiscountValue = null,
                            DiscountAmount = null,
                            MaxDiscountAmount = null
                        };

                        // Use CalculateEfficientCart to calculate discounts
                        var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString());
                        var discountTypeName = discountType?.Name ?? EDiscountType.UNKNOWN.Name;
                        var discountValue = (decimal)(coupon.DiscountValue ?? 0);
                        var maxDiscountAmount = coupon.MaxDiscountAmount.HasValue ? (decimal?)coupon.MaxDiscountAmount.Value : null;

                        var afterCart = CalculatePrice.CalculateEfficientCart(
                            beforeCart: beforeCart,
                            discountType: discountTypeName,
                            discountValue: discountValue,
                            maxDiscountAmount: maxDiscountAmount);

                        // Store cart-level promotion data for integration event
                        cartPromotionId = coupon.Id;
                        cartPromotionType = EPromotionType.COUPON.Name;
                        cartDiscountType = discountTypeName;
                        cartDiscountValue = discountValue;
                        cartDiscountAmount = afterCart.DiscountAmount;

                        // Update ShoppingCart discount properties
                        checkoutShoppingCart.DiscountType = discountTypeName;
                        checkoutShoppingCart.DiscountValue = discountValue;
                        checkoutShoppingCart.DiscountAmount = afterCart.DiscountAmount;
                        checkoutShoppingCart.MaxDiscountAmount = maxDiscountAmount;
                        checkoutShoppingCart.PromotionId = coupon.Id;
                        checkoutShoppingCart.PromotionType = EPromotionType.COUPON.Name;

                        // Map calculated discounts back to CartItems and apply coupons
                        for (int i = 0; i < selectedItems.Count; i++)
                        {
                            var cartItem = selectedItems[i];
                            var efficientItem = afterCart.CartItems[i];

                            PromotionCoupon promotionCoupon = PromotionCoupon.Create(
                                promotionId: coupon.Id,
                                promotionType: EPromotionType.COUPON.Name,
                                discountType: discountType ?? EDiscountType.UNKNOWN,
                                discountValue: efficientItem.DiscountValue ?? 0
                            );

                            cartItem.ApplyCoupon(promotionCoupon);
                            
                            // Update cart item discount amount
                            cartItem.DiscountAmount = efficientItem.DiscountAmount;
                        }
                    }
                }
            }
        }


        var parseOrderIdResult = Guid.TryParse(request.CryptoUUID, out var orderId);

        if (!parseOrderIdResult)
        {
            return Errors.Basket.InvalidCryptoUUID;
        }

        var orderItems = checkoutShoppingCart.CartItems
            .Select(x => x.ToCheckoutItemIntegrationEvent())
            .ToList();

        var integrationEventMessage = new BasketCheckoutIntegrationEvent
        {
            OrderId = orderId,
            TenantId = tenantId,
            BranchId = branchId,
            CustomerId = userId,
            CustomerEmail = userEmail,
            CustomerPublicKey = request.CustomerPublicKey,
            Tx = request.Tx,
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

        //await _basketRepository.DeleteBasketAsync(userEmail, cancellationToken);

        return new CheckoutBasketResponse()
        {
            OrderId = orderId.ToString(),
            CartItems = checkoutShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
            OrderDetailsRedirectUrl = $"/account/orders/{orderId}"
        };
    }
}
