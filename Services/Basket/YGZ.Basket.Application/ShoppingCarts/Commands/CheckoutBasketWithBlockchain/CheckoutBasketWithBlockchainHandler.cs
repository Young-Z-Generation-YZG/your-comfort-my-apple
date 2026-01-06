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
    private const string _defaultTenantId = "664355f845e56534956be32b";
    private const string _defaultBranchId = "664357a235e84033bbd0e6b6";

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
        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(CheckoutBasketWithBlockchainHandler), "Start checking out basket...", request);

        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();
        var tenantId = _tenantContext.GetTenantId() ?? _defaultTenantId;
        var branchId = _tenantContext.GetBranchId() ?? _defaultBranchId;

        // 1. check basket
        // 1.1. return error if cart is empty or null
        // 1.2. return error if no items selected

        var shoppingCartResult = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (shoppingCartResult.IsFailure)
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:IsFailure][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(CheckoutBasketWithBlockchainHandler), nameof(_basketRepository.GetBasketAsync), shoppingCartResult.Error.Message, new { userEmail, error = shoppingCartResult.Error });
            return shoppingCartResult.Error;
        }

        var shoppingCart = shoppingCartResult.Response!;

        // 1.1.
        if (!shoppingCart.CartItems.Any())
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(CheckoutBasketWithBlockchainHandler), nameof(_basketRepository.GetBasketAsync), Errors.Basket.BasketEmpty.Message, new { userEmail });
            return Errors.Basket.BasketEmpty;
        }

        // 1.2.
        if (shoppingCart.CartItems.All(ci => ci.IsSelected == false))
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(CheckoutBasketWithBlockchainHandler), nameof(_basketRepository.GetBasketAsync), Errors.Basket.NotSelectedItems.Message, new { userEmail, cartItemCount = shoppingCart.CartItems.Count });

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

            var discountGrpcRequest = new GetEventItemByIdRequest
            {
                EventItemId = cartPromotionId
            };

            EventItemModel? eventItemGrpc = null;

            try
            {
                _logger.LogInformation("===[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), "Getting event item by ID...", new { eventItemId = cartPromotionId });

                eventItemGrpc = await _discountProtoServiceClient.GetEventItemByIdGrpcAsync(discountGrpcRequest, cancellationToken: cancellationToken);
            }
            catch (RpcException ex)
            {
                _logger.LogCritical(ex, ":::[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), ex.Message, new { eventItemId = cartPromotionId });
                throw;
            }

            if (eventItemGrpc is null)
            {
                _logger.LogCritical(":::[CommandHandler:{CommandHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(InvalidOperationException), Errors.Discount.EventItemNotFound.Message, new { eventItemId = cartPromotionId });

                throw new InvalidOperationException("Event item not found");
            }

            var skuId = eventItemGrpc.SkuId;

            if (string.IsNullOrEmpty(skuId))
            {
                _logger.LogCritical(":::[CommandHandler:{CommandHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(InvalidOperationException), "Event item has no SKU ID", new { eventItemId = cartPromotionId });
                throw new InvalidOperationException("Event item has no SKU ID");
            }


            SkuModel? skuGrpc = null;

            try
            {
                _logger.LogInformation("===[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(CatalogProtoService), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), "Getting SKU by ID...", new { skuId = skuId });

                skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                {
                    SkuId = skuId
                }, cancellationToken: cancellationToken);
            }
            catch (RpcException ex)
            {
                _logger.LogCritical(ex, ":::[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(CatalogProtoService), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, new { skuId = skuId });
                throw;
            }

            if (skuGrpc is null)
            {
                _logger.LogCritical(":::[CommandHandler:{CommandHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(InvalidOperationException), "SKU not found", new { skuId = skuId });
                throw new InvalidOperationException("SKU not found");
            }

            if (skuGrpc.ReservedForEvent is null)
            {
                _logger.LogCritical(":::[CommandHandler:{CommandHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(InvalidOperationException), "SKU is not reserved for event", new { skuId = skuId });
                throw new InvalidOperationException("SKU is not reserved for event");
            }

            if (skuGrpc.ReservedForEvent.EventItemId != cartPromotionId)
            {
                _logger.LogCritical(":::[CommandHandler:{CommandHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(InvalidOperationException), "SKU is not reserved for this event", new { skuId = skuId });
                throw new InvalidOperationException("SKU is not reserved for this event");
            }

            var availableQuantitySku = skuGrpc.ReservedForEvent.ReservedQuantity - eventItemGrpc.Sold;
            var availableQuantityEventItem = eventItemGrpc.Stock - eventItemGrpc.Sold;

            if (availableQuantitySku <= 0 || availableQuantityEventItem <= 0 || (availableQuantitySku != availableQuantityEventItem))
            {
                _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(CheckoutBasketWithBlockchainHandler), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), Errors.Basket.InsufficientQuantity.Message, new { skuId = eventItemGrpc.SkuId, availableQuantitySku = availableQuantitySku, availableQuantityEventItem = availableQuantityEventItem });
                return Errors.Basket.InsufficientQuantity;
            }
        }
        // 2.2
        else
        {
            checkoutShoppingCart = shoppingCart.FilterOutEventItemsAndNotSelectedItem();

            // Check stock availability
            foreach (var cartItem in checkoutShoppingCart.CartItems)
            {
                SkuModel? skuGrpc = null;

                try
                {
                    _logger.LogInformation("===[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(CatalogProtoService), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), "Getting SKU by ID...", new { skuId = cartItem.SkuId });

                    skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                    {
                        SkuId = cartItem.SkuId
                    }, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    _logger.LogCritical(ex, ":::[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(CatalogProtoService), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, new { skuId = cartItem.SkuId });
                    throw;
                }

                if (skuGrpc is null)
                {
                    _logger.LogCritical(":::[CommandHandler:{CommandHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(CheckoutBasketWithBlockchainHandler), nameof(InvalidOperationException), "SKU not found", new { skuId = cartItem.SkuId });
                    throw new InvalidOperationException("SKU not found");
                }

                if (skuGrpc.AvailableInStock < cartItem.Quantity)
                {
                    _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), Errors.Basket.InsufficientQuantity.Message, new { skuId = cartItem.SkuId, availableQuantity = skuGrpc.AvailableInStock, requestedQuantity = cartItem.Quantity });
                    return Errors.Basket.InsufficientQuantity;
                }
            }

            // 2.2. (alter flow)
            if (!string.IsNullOrEmpty(request.DiscountCode))
            {
                CouponModel? couponGrpc = null;

                try
                {
                    _logger.LogInformation("===[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), "Getting coupon by code...", new { discountCode = request.DiscountCode });

                    couponGrpc = await _discountProtoServiceClient.GetCouponByCodeGrpcAsync(new GetCouponByCodeRequest
                    {
                        CouponCode = request.DiscountCode
                    }, cancellationToken: cancellationToken);
                }
                catch (RpcException ex)
                {
                    _logger.LogCritical(ex, ":::[CommandHandler:{CommandHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), ex.Message, new { discountCode = request.DiscountCode });
                    throw;
                }

                if (couponGrpc is null)
                {
                    _logger.LogWarning(":::[CommandHandler:{CommandHandler}][Warning:{Error}]::: Error message: {Message}, Parameters: {@Parameters}",
                        nameof(CheckoutBasketWithBlockchainHandler), nameof(Errors.Discount.CouponNotFound), Errors.Discount.CouponNotFound.Message, new { discountCode = request.DiscountCode });

                    // note: do not return error here
                    checkoutShoppingCart.SetDiscountCouponError($"Coupon code {request.DiscountCode} is expired or not found");
                }
                else
                {
                    if (couponGrpc.AvailableQuantity <= 0)
                    {
                        _logger.LogWarning(":::[CommandHandler:{CommandHandler}]::: Warning message: {Message}, Parameters: {@Parameters}",
                            nameof(CheckoutBasketWithBlockchainHandler), "Coupon is out of stock during checkout", new { discountCode = request.DiscountCode, availableQuantity = couponGrpc.AvailableQuantity, userEmail });

                        // note: do not return error here
                        checkoutShoppingCart.SetDiscountCouponError($"Coupon code {request.DiscountCode} is out of stock");
                    }
                    else
                    {
                        checkoutShoppingCart = HandleFinalCart(checkoutShoppingCart, couponGrpc!);

                        cartPromotionId = checkoutShoppingCart.PromotionId;
                        cartPromotionType = checkoutShoppingCart.PromotionType;
                        cartDiscountType = checkoutShoppingCart.DiscountType;
                        cartDiscountValue = checkoutShoppingCart.DiscountValue;
                        cartDiscountAmount = checkoutShoppingCart.DiscountAmount;
                    }
                }
            }
        }


        var parseOrderIdResult = Guid.TryParse(request.CryptoUUID, out var orderId);

        if (!parseOrderIdResult)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {Message}, Parameters: {@Parameters}",
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



        _logger.LogWarning("###[IntegrationEvent:{IntegrationEvent}]### Parameters: {@Parameters}", nameof(BasketCheckoutIntegrationEvent), integrationEventMessage);
        await _publishIntegrationEvent.Publish(integrationEventMessage, cancellationToken);

        await _basketRepository.DeleteSelectedItemsBasketAsync(userEmail, cancellationToken);

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

        // Convert discount value from decimal form (0.1 = 10%) to percentage form (10 = 10%)
        // CalculateEfficientCart expects percentage form for PERCENTAGE discount type
        // If value is between 0 and 1 (exclusive), it's in decimal form and needs conversion
        if (discountType == EDiscountType.PERCENTAGE && discountValue > 0 && discountValue < 1)
        {
            discountValue = discountValue * 100m;
        }

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
