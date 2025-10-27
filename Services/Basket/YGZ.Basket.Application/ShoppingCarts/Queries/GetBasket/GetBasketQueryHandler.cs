using Grpc.Core;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserRequestContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetBasketQueryHandler(IBasketRepository basketRepository, IUserRequestContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

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
                    // Step 1: Calculate total cart value for selected items
                    decimal totalCartValue = selectedItems.Sum(item => item.UnitPrice * item.Quantity);

                    // Step 2: Calculate discount amount
                    var discountType = ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString());
                    decimal calculatedDiscount = 0;

                    if (discountType == EDiscountType.PERCENTAGE)
                    {
                        // For percentage: discount = totalCartValue * (discountValue)
                        // Note: discountValue should already be in decimal form (e.g., 0.1 for 10%)
                        calculatedDiscount = totalCartValue * (decimal)(coupon.DiscountValue ?? 0);
                    }
                    else if (discountType == EDiscountType.FIXED_AMOUNT)
                    {
                        calculatedDiscount = (decimal)(coupon.DiscountValue ?? 0);
                    }

                    // Step 3: Apply maximum discount cap
                    decimal actualTotalDiscount = coupon.MaxDiscountAmount.HasValue && coupon.MaxDiscountAmount > 0
                        ? Math.Min(calculatedDiscount, (decimal)coupon.MaxDiscountAmount)
                        : calculatedDiscount;

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


        return new GetBasketResponse()
        {
            UserEmail = FilterOutEventItemsShoppingCart.UserEmail,
            CartItems = FilterOutEventItemsShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
            TotalAmount = FilterOutEventItemsShoppingCart.TotalAmount
        };
    }
}