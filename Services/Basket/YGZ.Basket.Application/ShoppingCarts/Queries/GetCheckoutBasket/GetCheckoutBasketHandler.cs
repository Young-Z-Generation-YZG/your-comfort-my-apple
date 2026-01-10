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

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetCheckoutBasket;

public class GetCheckoutBasketHandler : IQueryHandler<GetCheckoutBasketQuery, GetBasketResponse>
{
    private readonly ILogger<GetCheckoutBasketHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;

    public GetCheckoutBasketHandler(IBasketRepository basketRepository,
                                    IUserHttpContext userContext,
                                    DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
                                    ILogger<GetCheckoutBasketHandler> logger,
                                    CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
        _logger = logger;
        _catalogProtoServiceClient = catalogProtoServiceClient;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetCheckoutBasketQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[QueryHandler:{QueryHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(GetCheckoutBasketHandler), "Start retrieving checkout basket...", request);

        var userEmail = _userContext.GetUserEmail();

        var getBaketResult = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (getBaketResult.IsFailure)
        {
            _logger.LogError(":::[QueryHandler:{QueryHandler}][Result:IsFailure][Method:{MethodName}]::: Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GetCheckoutBasketHandler), nameof(_basketRepository.GetBasketAsync), getBaketResult.Error.Message, request);
            return getBaketResult.Error;
        }

        var shoppingCart = getBaketResult.Response!;

        if (!shoppingCart.CartItems.Any())
        {
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
                    EventItemModel? eventItemGrpc = null;
                    try
                    {
                        _logger.LogInformation("===[QueryHandler:{QueryHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), "Getting event item by ID...", new { eventItemId = eventPromotion.PromotionId });

                        eventItemGrpc = await _discountProtoServiceClient.GetEventItemByIdGrpcAsync(new GetEventItemByIdRequest
                        {
                            EventItemId = eventPromotion.PromotionId
                        }, cancellationToken: cancellationToken);
                    }
                    catch (RpcException ex)
                    {
                        _logger.LogCritical(ex, ":::[QueryHandler:{QueryHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetEventItemByIdGrpcAsync), ex.Message, new { eventItemId = eventPromotion.PromotionId });
                        throw;
                    }

                    if (eventItemGrpc is null)
                    {
                        _logger.LogCritical(":::[QueryHandler:{QueryHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(InvalidOperationException), Errors.Discount.EventItemNotFound.Message, new { eventItemId = eventPromotion.PromotionId });

                        throw new InvalidOperationException("Event item not found");
                    }

                    if (string.IsNullOrEmpty(eventItemGrpc.SkuId))
                    {
                        _logger.LogCritical(":::[QueryHandler:{QueryHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(InvalidOperationException), "Event item has no SKU ID", new { eventItemId = eventPromotion.PromotionId });

                        throw new InvalidOperationException("Event item has no SKU ID");
                    }

                    SkuModel? skuGrpc = null;

                    try
                    {
                        _logger.LogInformation("===[QueryHandler:{QueryHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(CatalogProtoService), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), "Getting SKU by ID...", new { skuId = eventItemGrpc.SkuId });

                        skuGrpc = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                        {
                            SkuId = eventItemGrpc.SkuId
                        }, cancellationToken: cancellationToken);
                    }
                    catch (RpcException ex)
                    {
                        _logger.LogCritical(ex, ":::[QueryHandler:{QueryHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(CatalogProtoService), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, new { skuId = eventItemGrpc.SkuId });
                        throw;
                    }

                    if (skuGrpc is null)
                    {
                        _logger.LogCritical(":::[QueryHandler:{QueryHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(InvalidOperationException), "SKU not found", new { skuId = eventItemGrpc.SkuId });
                        throw new InvalidOperationException("SKU not found");
                    }

                    if (skuGrpc.ReservedForEvent is null)
                    {
                        _logger.LogCritical(":::[QueryHandler:{QueryHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(InvalidOperationException), "SKU is not reserved for event", new { skuId = eventItemGrpc.SkuId });
                        throw new InvalidOperationException("SKU is not reserved for event");
                    }

                    if (skuGrpc.ReservedForEvent.EventItemId != eventPromotion.PromotionId)
                    {
                        _logger.LogCritical(":::[QueryHandler:{QueryHandler}][Exception:{Eception}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(InvalidOperationException), "SKU is not reserved for this event", new { skuId = eventItemGrpc.SkuId });
                        throw new InvalidOperationException("SKU is not reserved for this event");
                    }

                    var availableQuantitySku = skuGrpc.ReservedForEvent.ReservedQuantity;
                    var availableQuantityEventItem = eventItemGrpc.Stock - eventItemGrpc.Sold;

                    if (availableQuantitySku <= 0 || availableQuantityEventItem <= 0 || (availableQuantitySku != availableQuantityEventItem))
                    {
                        _logger.LogError(":::[QueryHandler:{QueryHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                            nameof(GetCheckoutBasketHandler), nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), Errors.Basket.InsufficientQuantity.Message, new { skuId = eventItemGrpc.SkuId, availableQuantitySku = availableQuantitySku, availableQuantityEventItem = availableQuantityEventItem });

                        return Errors.Basket.InsufficientQuantity;
                    }

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
        // Note: For checkout, we only return selected items
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            if (!finalCart.CartItems.Any())
            {
                _logger.LogError(":::[QueryHandler:{QueryHandler}][Result:Error][Method:{MethodName}]::: Error message: {ErrorMessage}, Parameters: {@Parameters}",
                  nameof(GetCheckoutBasketHandler), nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), Errors.Basket.NotSelectedItems.Message, new { couponCode = request.CouponCode });
                return Errors.Basket.NotSelectedItems;
            }
            // Filter to only selected items for checkout
            finalCart = filterOutEventItemsShoppingCart.FilterSelectedItem();

            if (!finalCart.CartItems.Any())
            {
                finalCart.SetCouponApplied(
                    errorMessage: "Selected item to apply coupon",
                    title: null,
                    discountType: null,
                    discountValue: null,
                    maxDiscountAmount: null,
                    description: null,
                    expiredDate: null);
            }

            CouponModel? couponGrpc = null;

            try
            {
                _logger.LogInformation("===[QueryHandler:{QueryHandler}][gRPC:{gRPCName}][Method:{MethodName}]=== Information message: {Message}, Parameters: {@Parameters}",
                    nameof(GetCheckoutBasketHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), "Getting coupon by code...", new { couponCode = request.CouponCode });

                couponGrpc = await _discountProtoServiceClient.GetCouponByCodeGrpcAsync(new GetCouponByCodeRequest
                {
                    CouponCode = request.CouponCode
                }, cancellationToken: cancellationToken);
            }
            catch (RpcException ex)
            {
                _logger.LogCritical(ex, ":::[QueryHandler:{QueryHandler}][gRPC:{gRPCName}][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                    nameof(GetCheckoutBasketHandler), nameof(DiscountProtoService), nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), ex.Message, new { couponCode = request.CouponCode });
                throw;
            }

            if (couponGrpc is null)
            {
                _logger.LogWarning(":::[QueryHandler:{QueryHandler}][Warning:{Warning}]::: Warning message: {Message}, Parameters: {@Parameters}",
                    nameof(GetCheckoutBasketHandler), nameof(Errors.Discount.CouponNotFound), Errors.Discount.CouponNotFound.Message, new { couponCode = request.CouponCode });

                // note: do not return error here
                finalCart.SetCouponApplied(
                    errorMessage: $"Coupon code {request.CouponCode} is expired or not found",
                    title: null,
                    discountType: null,
                    discountValue: null,
                    maxDiscountAmount: null,
                    description: null,
                    expiredDate: null
                );
            }
            else
            {
                finalCart = HandleFinalCart(finalCart, couponGrpc);

                // 4.1.4 - Apply discount to selected items (cart already filtered to selected items only)
                finalCart = HandleFinalCart(finalCart, couponGrpc);

                // Set coupon applied information with discount details
                var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(couponGrpc.DiscountType.ToString());
                var discountTypeName = discountType?.Name ?? EDiscountType.UNKNOWN.Name;
                var discountValue = (decimal)(couponGrpc.DiscountValue ?? 0);

                // Convert discount value from decimal form (0.1 = 10%) to percentage form (10 = 10%) for display
                if (discountType == EDiscountType.PERCENTAGE && discountValue > 0 && discountValue < 1)
                {
                    discountValue = discountValue * 100m;
                }

                // Convert from gRPC Timestamp (UTC+7) back to UTC DateTime
                // ToTimestampUtc adds 7 hours, so we subtract 7 hours when converting back
                var expiredDateUtc = couponGrpc.ExpiredDate?.ToDateTime();
                var expiredDate = expiredDateUtc.HasValue
                    ? expiredDateUtc.Value.AddHours(-7).ToUniversalTime()
                    : (DateTime?)null;

                finalCart.SetCouponApplied(
                    errorMessage: null,
                    title: couponGrpc.Title ?? null,
                    discountType: discountTypeName,
                    discountValue: (double?)discountValue,
                    maxDiscountAmount: couponGrpc.MaxDiscountAmount.HasValue ? (double?)couponGrpc.MaxDiscountAmount.Value : null,
                    description: couponGrpc.Description ?? null,
                    expiredDate: expiredDate);
            }

















            // var grpcRequest = new GetCouponByCodeRequest
            // {
            //     CouponCode = request.CouponCode
            // };

            // CouponModel? coupon = null;

            // // 4.1
            // try
            // {
            //     coupon = await _discountProtoServiceClient.GetCouponByCodeGrpcAsync(grpcRequest, cancellationToken: cancellationToken);
            // }
            // catch (RpcException ex)
            // {
            //     // Discount Service handles all validation - trust the error from Discount Service
            //     if (ex.StatusCode == StatusCode.NotFound || ex.StatusCode == StatusCode.FailedPrecondition)
            //     {
            //         if (finalCart.CartItems.Any())
            //         {
            //             // Get error message from Discount Service
            //             var errorMessage = ex.Message;

            //             _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
            //                 nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), "Coupon validation failed", new { couponCode = request.CouponCode, statusCode = ex.StatusCode, errorMessage, userEmail });

            //             finalCart.SetCouponApplied(
            //                 errorMessage: errorMessage,
            //                 title: null,
            //                 discountType: null,
            //                 discountValue: null,
            //                 maxDiscountAmount: null,
            //                 description: null,
            //                 expiredDate: null);
            //         }
            //     }
            //     else
            //     {
            //         var parameters = new { couponCode = request.CouponCode, userEmail };
            //         _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
            //             nameof(_discountProtoServiceClient.GetCouponByCodeGrpcAsync), ex.Message, parameters);
            //     }
            // }

            // if (couponGrpc is not null)
            // {
            //     // Coupon is valid (Discount Service already validated it)
            //     // 4.1.3 - Check if items are selected
            //     if (!finalCart.CartItems.Any())
            //     {
            //         _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
            //             nameof(Handle), "No selected items to apply coupon", new { couponCode = request.CouponCode, userEmail });

            //         return Errors.Basket.NotSelectedItems;
            //     }

            //     // 4.1.4 - Apply discount to selected items (cart already filtered to selected items only)
            //     finalCart = HandleFinalCart(finalCart, coupon);

            //     // Set coupon applied information with discount details
            //     var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString());
            //     var discountTypeName = discountType?.Name ?? EDiscountType.UNKNOWN.Name;
            //     var discountValue = (decimal)(coupon.DiscountValue ?? 0);

            //     // Convert discount value from decimal form (0.1 = 10%) to percentage form (10 = 10%) for display
            //     if (discountType == EDiscountType.PERCENTAGE && discountValue > 0 && discountValue < 1)
            //     {
            //         discountValue = discountValue * 100m;
            //     }

            //     // Convert from gRPC Timestamp (UTC+7) back to UTC DateTime
            //     // ToTimestampUtc adds 7 hours, so we subtract 7 hours when converting back
            //     var expiredDateUtc = coupon.ExpiredDate?.ToDateTime();
            //     var expiredDate = expiredDateUtc.HasValue
            //         ? expiredDateUtc.Value.AddHours(-7).ToUniversalTime()
            //         : (DateTime?)null;

            //     finalCart.SetCouponApplied(
            //         errorMessage: null,
            //         title: coupon.Title ?? null,
            //         discountType: discountTypeName,
            //         discountValue: (double?)discountValue,
            //         maxDiscountAmount: coupon.MaxDiscountAmount.HasValue ? (double?)coupon.MaxDiscountAmount.Value : null,
            //         description: coupon.Description ?? null,
            //         expiredDate: expiredDate);

            //     _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {Message}, Parameters: {@Parameters}",
            //         nameof(Handle), "Successfully applied coupon to checkout basket", new { couponCode = request.CouponCode, userEmail, cartItemCount = finalCart.CartItems.Count });
            // }
        }
        else
        {
            // If no coupon code, still filter to only selected items for checkout
            finalCart = filterOutEventItemsShoppingCart.FilterSelectedItem();
        }

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
