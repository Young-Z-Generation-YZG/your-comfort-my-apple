using Grpc.Core;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetCheckoutBasket;

public class GetCheckoutBasketHandler : IQueryHandler<GetCheckoutBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserRequestContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetCheckoutBasketHandler(IBasketRepository basketRepository, IUserRequestContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetCheckoutBasketQuery request, CancellationToken cancellationToken)
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

        if (shoppingCart.CheckHasEventItems())
        {
            ShoppingCart FilterEventItemsShoppingCart = shoppingCart.FilterEventItems();

            return new GetBasketResponse()
            {
                UserEmail = FilterEventItemsShoppingCart.UserEmail,
                CartItems = FilterEventItemsShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                TotalAmount = FilterEventItemsShoppingCart.TotalAmount
            };
        }

        if (shoppingCart.CartItems.All(ci => ci.IsSelected == false))
        {
            return Errors.Basket.NotSelectedItems;
        }

        ShoppingCart FilterOutEventItemsShoppingCart = shoppingCart.FilterOutEventItemsAndSelected();

        // get coupon details if coupon code is provided
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            var grpcRequest = new GetCouponByCodeRequest
            {
                CouponCode = request.CouponCode
            };

            CouponModel coupon = null;

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

            if (coupon != null)
            {
                var quantity = coupon.AvailableQuantity;

                foreach (var item in FilterOutEventItemsShoppingCart.CartItems)
                {
                    if (item.IsSelected == true)
                    {
                        PromotionCoupon promotionCoupon = PromotionCoupon.Create(couponId: coupon.Id, productUnitPrice: item.UnitPrice, discountType: ConvertGrpcEnumToNormalEnum.ConvertToEDiscountType(coupon.DiscountType.ToString()), discountValue: (decimal)coupon.DiscountValue, discountAmount: (decimal)coupon.DiscountValue);

                        item.ApplyCoupon(promotionCoupon);

                        quantity--;
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
