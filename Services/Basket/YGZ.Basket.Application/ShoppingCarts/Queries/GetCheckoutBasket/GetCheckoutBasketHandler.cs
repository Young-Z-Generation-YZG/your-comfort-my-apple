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
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetCheckoutBasket;

public class GetCheckoutBasketHandler : IQueryHandler<GetCheckoutBasketQuery, GetBasketResponse>
{
    private readonly ILogger<GetCheckoutBasketHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetCheckoutBasketHandler(IBasketRepository basketRepository,
                                    IUserHttpContext userContext,
                                    DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
                                    ILogger<GetCheckoutBasketHandler> logger)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetCheckoutBasketQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.GetBasketAsync), "Failed to retrieve basket from repository", new { userEmail, error = result.Error });

            return result.Error;
        }

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Basket is empty, returning empty checkout response", new { userEmail });

            return new GetBasketResponse()
            {
                UserEmail = shoppingCart.UserEmail,
                CartItems = new List<CartItemResponse>(),
                SubTotalAmount = 0,
                PromotionId = null,
                PromotionType = null,
                DiscountType = null,
                DiscountValue = null,
                DiscountAmount = null,
                MaxDiscountAmount = null,
                TotalAmount = 0
            };
        }

        if (shoppingCart.CheckHasEventItems())
        {
            ShoppingCart FilterEventItemsShoppingCart = shoppingCart.FilterEventItems();

            if (FilterEventItemsShoppingCart.CartItems.Any())
            {
                // Extract promotion information from event items
                var firstEventItem = FilterEventItemsShoppingCart.CartItems.FirstOrDefault();
                var eventPromotion = firstEventItem?.Promotion?.PromotionEvent;

                if (eventPromotion is not null)
                {
                    var efficientCartItems = FilterEventItemsShoppingCart.CartItems.Select((item, index) => new EfficientCartItem
                    {
                        UniqueString = $"item_{index}_{item.GetHashCode()}",
                        OriginalPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        PromotionId = item.Promotion?.PromotionEvent?.PromotionId,
                        PromotionType = EPromotionType.EVENT_ITEM.Name,
                        DiscountType = null,
                        DiscountValue = null,
                        DiscountAmount = null
                    }).ToList();

                    var beforeCart = new EfficientCart
                    {
                        CartItems = efficientCartItems,
                        PromotionId = eventPromotion.PromotionId,
                        PromotionType = EPromotionType.EVENT_ITEM.Name,
                        DiscountType = null,
                        DiscountValue = null,
                        DiscountAmount = null,
                        MaxDiscountAmount = null
                    };

                    var discountTypeName = eventPromotion.DiscountType;
                    var discountValue = eventPromotion.DiscountValue;

                    var afterCart = CalculatePrice.CalculateEfficientCart(
                        beforeCart: beforeCart,
                        discountType: discountTypeName,
                        discountValue: discountValue,
                        maxDiscountAmount: null);

                    FilterEventItemsShoppingCart.PromotionId = afterCart.PromotionId;
                    FilterEventItemsShoppingCart.PromotionType = afterCart.PromotionType;
                    FilterEventItemsShoppingCart.DiscountType = afterCart.DiscountType;
                    FilterEventItemsShoppingCart.DiscountValue = afterCart.DiscountValue;
                    FilterEventItemsShoppingCart.DiscountAmount = afterCart.DiscountAmount;
                    FilterEventItemsShoppingCart.MaxDiscountAmount = afterCart.MaxDiscountAmount;

                    for (int i = 0; i < FilterEventItemsShoppingCart.CartItems.Count && i < afterCart.CartItems.Count; i++)
                    {
                        var cartItem = FilterEventItemsShoppingCart.CartItems[i];
                        var efficientItem = afterCart.CartItems[i];
                        cartItem.DiscountAmount = efficientItem.DiscountAmount;
                    }

                    return new GetBasketResponse()
                    {
                        UserEmail = FilterEventItemsShoppingCart.UserEmail,
                        CartItems = FilterEventItemsShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                        SubTotalAmount = FilterEventItemsShoppingCart.SubTotalAmount,
                        PromotionId = FilterEventItemsShoppingCart.PromotionId,
                        PromotionType = FilterEventItemsShoppingCart.PromotionType,
                        DiscountType = FilterEventItemsShoppingCart.DiscountType,
                        DiscountValue = FilterEventItemsShoppingCart.DiscountValue,
                        DiscountAmount = FilterEventItemsShoppingCart.DiscountAmount,
                        MaxDiscountAmount = FilterEventItemsShoppingCart.MaxDiscountAmount,
                        TotalAmount = FilterEventItemsShoppingCart.TotalAmount
                    };
                }
            }
        }

        // 3. Filter out event items from shopping cart
        ShoppingCart filterOutEventItemsShoppingCart = shoppingCart.FilterOutEventItems();
        ShoppingCart finalCart = filterOutEventItemsShoppingCart; // override final cart after calculate price with coupon

        // 4 handle calculate cart price
        // 4.1 (alter flow) get coupon details if coupon code is provided
        // 4.1.1. SetDiscountCouponError if coupon not found or no available quantity
        // 4.1.2. if coupon found and available quantity > 0, calculate discount and apply to cart and cart items
        // 4.1.3. if no selected items, return error
        // 4.1.4. apply discount to selected items
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            var selectedItems = filterOutEventItemsShoppingCart.CartItems
                    .Where(item => item.IsSelected == true)
                    .ToList();

            if (!selectedItems.Any())
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
                    if (selectedItems.Any())
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
                    if (!selectedItems.Any())
                    {
                        _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                            nameof(Handle), "No selected items to apply coupon", new { couponCode = request.CouponCode, userEmail });

                        return Errors.Basket.NotSelectedItems;
                    }

                    // 4.1.4 
                    finalCart = HandleFinalCart(finalCart, coupon);

                    _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                        nameof(Handle), "Successfully applied coupon to checkout basket", new { couponCode = request.CouponCode, userEmail, cartItemCount = finalCart.CartItems.Count });
                }
            }
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved checkout basket", new { userEmail, cartItemCount = finalCart.CartItems.Count, totalAmount = finalCart.TotalAmount });

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
