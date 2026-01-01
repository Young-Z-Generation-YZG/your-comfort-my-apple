using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Api.Protos;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly ILogger<GetBasketHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;

    public GetBasketHandler(IBasketRepository basketRepository, IUserHttpContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient, CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient, ILogger<GetBasketHandler> logger)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _logger = logger;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        // 1. get basket
        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.GetBasketAsync), "Failed to retrieve basket from repository", new { userEmail, error = result.Error });

            return result.Error;
        }

        var shoppingCart = result.Response!;

        // 3. Filter out event items from shopping cart
        ShoppingCart filterOutEventItemsShoppingCart = shoppingCart.FilterOutEventItems();
        ShoppingCart finalCart = filterOutEventItemsShoppingCart; // override final cart after calculate price with coupon
        finalCart.PromotionId = null;
        finalCart.DiscountType = null;
        finalCart.DiscountValue = null;
        finalCart.DiscountAmount = null;

        // 1.1 return if cart is empty
        if (!finalCart.CartItems.Any())
        {
            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Basket is empty, returning empty response", new { userEmail });

            return new GetBasketResponse()
            {
                UserEmail = shoppingCart.UserEmail,
                CartItems = new List<CartItemResponse>(),
                TotalAmount = 0
            };
        }



        // 2 loop cart items to check stock availability
        // 2.1 if any sku stock is insufficient, set IsOutOfStock = true
        foreach (var cartItem in finalCart.CartItems)
        {
            try
            {
                var skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                {
                    SkuId = cartItem.SkuId
                }, cancellationToken: cancellationToken);

                SkuModel sku;


                // check sku
                if (skuGrpc is not null)
                {
                    sku = skuGrpc;
                    // check stock availability
                    if (skuGrpc.AvailableInStock < cartItem.Quantity)
                    {
                        cartItem.SetOutOfStock(true);
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



        // 4 handle calculate cart price
        // 4.1 (alter flow) get coupon details if coupon code is provided
        // 4.1.1. SetDiscountCouponError if coupon not found or no available quantity
        // 4.1.2. if coupon found and available quantity > 0, calculate discount and apply to cart and cart items
        // 4.1.3. if no selected items, return error
        // 4.1.4. apply discount to selected items
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            finalCart = filterOutEventItemsShoppingCart.FilterSelectedItem();

            if (!finalCart.CartItems.Any())
            {
                finalCart.SetDiscountCouponError("Selected item to apply coupon");
            }

            var grpcRequest = new GetCouponByCodeRequest
            {
                CouponCode = request.CouponCode
            };

            CouponModel? coupon = null;

            // 4.1
            try
            {
                coupon = await _discountProtoServiceClient.GetCouponByCodeGrpcAsync(grpcRequest, cancellationToken: cancellationToken);
            }
            catch (RpcException ex)
            {
                // 4.1.1
                if (ex.StatusCode == StatusCode.NotFound)
                {
                    if (finalCart.CartItems.Any())
                    {
                        _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                            nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), "Coupon not found", new { couponCode = request.CouponCode, userEmail });

                        finalCart.SetDiscountCouponError($"Coupon code {request.CouponCode} is expired or not found");
                    }
                }
                else
                {
                    var parameters = new { couponCode = request.CouponCode, userEmail };
                    _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), ex.Message, parameters);
                }
            }

            if (coupon is not null)
            {
                // 4.1.1
                if (coupon.AvailableQuantity <= 0)
                {
                    _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                        nameof(Handle), "Coupon has no available quantity", new { couponCode = request.CouponCode, availableQuantity = coupon.AvailableQuantity, userEmail });

                    finalCart.SetDiscountCouponError($"Coupon code {request.CouponCode} is expired or not found");
                }
                else
                {
                    // 4.1.3
                    if (!finalCart.CartItems.Any())
                    {
                        _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                            nameof(Handle), "No selected items to apply coupon", new { couponCode = request.CouponCode, userEmail });

                        return Errors.Basket.NotSelectedItems;
                    }

                    // 4.1.4 
                    finalCart = HandleFinalCart(finalCart, coupon);

                    _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                        nameof(Handle), "Successfully applied coupon to basket", new { couponCode = request.CouponCode, userEmail, cartItemCount = finalCart.CartItems.Count });
                }
            }
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved basket", new { userEmail, cartItemCount = finalCart.CartItems.Count, totalAmount = finalCart.TotalAmount });

        return finalCart.ToResponse();
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
