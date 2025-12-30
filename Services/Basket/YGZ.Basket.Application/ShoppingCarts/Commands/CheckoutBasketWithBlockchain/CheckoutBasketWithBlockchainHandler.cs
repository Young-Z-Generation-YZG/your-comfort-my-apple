using Grpc.Core;
using MassTransit;
using Microsoft.Extensions.Logging;
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
using YGZ.Catalog.Api.Protos;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.CheckoutBasketWithBlockchain;

public class CheckoutBasketWithBlockchainHandler : ICommandHandler<CheckoutBasketWithBlockchainCommand, CheckoutBasketResponse>
{
    private readonly ILogger<CheckoutBasketWithBlockchainHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IPublishEndpoint _publishIntegrationEvent;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;
    private readonly IUserHttpContext _userContext;
    private readonly ITenantHttpContext _tenantContext;

    public CheckoutBasketWithBlockchainHandler(IBasketRepository basketRepository,
                                               IPublishEndpoint publishEndpoint,
                                               IUserHttpContext userContext,
                                               ITenantHttpContext tenantContext,
                                               DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
                                               CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient,
                                               ILogger<CheckoutBasketWithBlockchainHandler> logger)
    {
        _basketRepository = basketRepository;
        _publishIntegrationEvent = publishEndpoint;
        _userContext = userContext;
        _tenantContext = tenantContext;
        _discountProtoServiceClient = discountProtoServiceClient;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _logger = logger;
    }


    public async Task<Result<CheckoutBasketResponse>> Handle(CheckoutBasketWithBlockchainCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();
        var tenantId = _tenantContext.GetTenantId() ?? "664355f845e56534956be32b";
        var branchId = _tenantContext.GetBranchId() ?? "664357a235e84033bbd0e6b6";

        // 1. check basket
        // 1.1. return error if cart is empty or null
        // 1.2. return error if no items selected

        var shoppingCartResult = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (shoppingCartResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.GetBasketAsync), "Failed to retrieve basket from repository", new { userEmail, error = shoppingCartResult.Error });

            return shoppingCartResult.Error;
        }

        var shoppingCart = shoppingCartResult.Response;

        // 1.1.
        if (shoppingCart is null || !shoppingCart.CartItems.Any())
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Basket is empty or null", new { userEmail });

            return Errors.Basket.BasketEmpty;
        }

        // 1.2.
        if (shoppingCart.CartItems.All(ci => ci.IsSelected == false))
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "No items selected for checkout", new { userEmail, cartItemCount = shoppingCart.CartItems.Count });

            return Errors.Basket.NotSelectedItems;
        }

        // 2. Handle checkout with event item or normal items
        // 2.1 CASE 1: if cart has event items, only checkout event items
        // 2.2 CASE 2: if cart has no event items, checkout selected normal items, apply coupon if provided
        // 2.2. (alter flow): handle coupon if provided

        // 2.1.
        ShoppingCart checkoutShoppingCart;
        string? cartPromotionId = null;
        string? cartPromotionType = null;
        string? cartDiscountType = null;
        decimal? cartDiscountValue = null;
        decimal? cartDiscountAmount = null;

        var eventItems = shoppingCart.CartItems.Where(ci => ci.Promotion?.PromotionEvent is not null).Where(ci => ci.IsSelected == true).ToList();

        if (eventItems.Any())
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
        // 2.2
        else
        {
            checkoutShoppingCart = shoppingCart.FilterOutEventItemsAndNotSelectedItem();

            // Check stock availability
            foreach (var cartItem in checkoutShoppingCart.CartItems)
            {
                try
                {
                    var skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                    {
                        SkuId = cartItem.SkuId
                    }, cancellationToken: cancellationToken);

                    SkuModel sku;

                    if (skuGrpc is not null)
                    {
                        sku = skuGrpc;

                        if (skuGrpc.AvailableInStock < cartItem.Quantity)
                        {
                            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                                nameof(Handle), "Insufficient stock for SKU during blockchain checkout", new { skuId = cartItem.SkuId, requestedQuantity = cartItem.Quantity, availableStock = skuGrpc.AvailableInStock, userEmail });

                            return Errors.Basket.InsufficientQuantity;
                        }
                    }
                }
                catch (RpcException ex)
                {
                    var parameters = new { skuId = cartItem.SkuId, userEmail };
                    _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, parameters);
                    throw;
                }
            }

            // 2.2. (alter flow)
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
                        if (checkoutShoppingCart.CartItems.Any())
                        {
                            _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                                nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), "Coupon not found during blockchain checkout", new { discountCode = request.DiscountCode, userEmail });

                            checkoutShoppingCart.SetDiscountCouponError($"Coupon code {request.DiscountCode} is expired or not found");
                        }
                    }
                    else
                    {
                        var parameters = new { discountCode = request.DiscountCode, userEmail };
                        _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                            nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), ex.Message, parameters);
                    }
                }

                if (coupon is not null)
                {
                    if (coupon.AvailableQuantity <= 0)
                    {
                        _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                            nameof(Handle), "Coupon has no available quantity during blockchain checkout", new { discountCode = request.DiscountCode, availableQuantity = coupon.AvailableQuantity, userEmail });

                        checkoutShoppingCart.SetDiscountCouponError($"Coupon code {request.DiscountCode} is expired or not found");
                    }
                    else
                    {
                        checkoutShoppingCart = HandleFinalCart(checkoutShoppingCart, coupon);

                        cartPromotionId = checkoutShoppingCart.PromotionId;
                        cartPromotionType = checkoutShoppingCart.PromotionType;
                        cartDiscountType = checkoutShoppingCart.DiscountType;
                        cartDiscountValue = checkoutShoppingCart.DiscountValue;
                        cartDiscountAmount = checkoutShoppingCart.DiscountAmount;

                        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                            nameof(Handle), "Successfully applied coupon during blockchain checkout", new { discountCode = request.DiscountCode, userEmail, discountAmount = cartDiscountAmount });
                    }
                }
            }
        }


        var parseOrderIdResult = Guid.TryParse(request.CryptoUUID, out var orderId);

        if (!parseOrderIdResult)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Invalid crypto UUID format", new { cryptoUUID = request.CryptoUUID, userEmail });

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

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully processed blockchain checkout", new { orderId, userEmail, paymentMethod = request.PaymentMethod, cartItemCount = checkoutShoppingCart.CartItems.Count });

        return new CheckoutBasketResponse()
        {
            OrderId = orderId.ToString(),
            CartItems = checkoutShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
            OrderDetailsRedirectUrl = $"/account/orders/{orderId}"
        };
    }

    private ShoppingCart HandleFinalCart(ShoppingCart cart, CouponModel coupon)
    {
        var efficientCartItems = cart.CartItems.Select((item, index) => new EfficientCartItem
        {
            UniqueString = $"item_{index}_{item.GetHashCode()}",
            OriginalPrice = item.UnitPrice,
            Quantity = item.Quantity,
            PromotionId = item.Promotion?.PromotionCoupon?.PromotionId,
            PromotionType = item.Promotion?.PromotionCoupon?.PromotionType,
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

        var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString());
        var discountTypeName = discountType?.Name ?? EDiscountType.UNKNOWN.Name;
        var discountValue = (decimal)(coupon.DiscountValue ?? 0);
        var maxDiscountAmount = coupon.MaxDiscountAmount.HasValue ? (decimal?)coupon.MaxDiscountAmount.Value : null;

        var afterCart = CalculatePrice.CalculateEfficientCart(
            beforeCart: beforeCart,
            discountType: discountTypeName,
            discountValue: discountValue,
            maxDiscountAmount: maxDiscountAmount);

        cart.PromotionId = afterCart.PromotionId;
        cart.PromotionType = afterCart.PromotionType;
        cart.DiscountType = afterCart.DiscountType;
        cart.DiscountValue = afterCart.DiscountValue;
        cart.DiscountAmount = afterCart.DiscountAmount;
        cart.MaxDiscountAmount = null;

        for (int i = 0; i < cart.CartItems.Count; i++)
        {
            var cartItem = cart.CartItems[i];
            var efficientItem = afterCart.CartItems[i];

            var promotionCoupon = PromotionCoupon.Create(
                promotionId: coupon.Id,
                promotionType: EPromotionType.COUPON.Name,
                discountType: discountType ?? EDiscountType.UNKNOWN,
                discountValue: efficientItem.DiscountValue ?? 0
            );

            cartItem.ApplyCoupon(promotionCoupon);
            cartItem.DiscountAmount = efficientItem.DiscountAmount;
        }

        return cart;
    }
}
