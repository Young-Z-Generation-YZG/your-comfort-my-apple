using Grpc.Core;
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

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetBasketQueryHandler(IBasketRepository basketRepository, IUserHttpContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            return new GetBasketResponse()
            {
                UserEmail = shoppingCart.UserEmail,
                CartItems = new List<CartItemResponse>(),
                TotalAmount = 0
            };
        }

        ShoppingCart FilterOutEventItemsShoppingCart = shoppingCart.FilterOutEventItems();

        // get coupon details if coupon code is provided
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            var grpcRequest = new GetCouponByCodeRequest
            {
                CouponCode = request.CouponCode
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


            if (coupon != null && coupon.AvailableQuantity > 0)
            {
                // Get only selected items
                var selectedItems = FilterOutEventItemsShoppingCart.CartItems
                    .Where(item => item.IsSelected == true)
                    .ToList();

                if (selectedItems.Any())
                {
                    // Convert CartItems to EfficientCart format
                    var efficientCartItems = selectedItems.Select((item, index) => new EfficientCartItem
                    {
                        UniqueString = $"item_{index}_{item.GetHashCode()}", // Unique identifier for mapping
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

                    // Update ShoppingCart discount properties from EfficientCart
                    FilterOutEventItemsShoppingCart.PromotionId = afterCart.PromotionId;
                    FilterOutEventItemsShoppingCart.PromotionType = afterCart.PromotionType;
                    FilterOutEventItemsShoppingCart.DiscountType = afterCart.DiscountType;
                    FilterOutEventItemsShoppingCart.DiscountValue = afterCart.DiscountValue;
                    FilterOutEventItemsShoppingCart.DiscountAmount = afterCart.DiscountAmount;
                    FilterOutEventItemsShoppingCart.MaxDiscountAmount = null;

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


        return new GetBasketResponse()
        {
            UserEmail = FilterOutEventItemsShoppingCart.UserEmail,
            CartItems = FilterOutEventItemsShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
            SubTotalAmount = FilterOutEventItemsShoppingCart.SubTotalAmount,
            PromotionId = FilterOutEventItemsShoppingCart.PromotionId,
            PromotionType = FilterOutEventItemsShoppingCart.PromotionType,
            DiscountType = FilterOutEventItemsShoppingCart.DiscountType,
            DiscountValue = FilterOutEventItemsShoppingCart.DiscountValue,
            DiscountAmount = FilterOutEventItemsShoppingCart.DiscountAmount,
            MaxDiscountAmount = FilterOutEventItemsShoppingCart.MaxDiscountAmount,
            TotalAmount = FilterOutEventItemsShoppingCart.TotalAmount
        };
    }
}