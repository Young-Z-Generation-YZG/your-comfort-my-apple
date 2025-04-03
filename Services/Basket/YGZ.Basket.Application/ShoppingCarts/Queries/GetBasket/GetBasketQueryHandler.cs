
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Enums;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly IUserContext _userContext;

    public GetBasketQueryHandler(IBasketRepository basketRepository, IUserContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync("lov3rinve146@gmail.com", cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        if(!string.IsNullOrEmpty(request.CouponCode)) {

            var couponDiscount = await _discountProtoServiceClient.GetDiscountByCodeAsync(new GetDiscountRequest { Code = request.CouponCode });

            if (couponDiscount is null)
            {
                return Errors.Discount.PromotionCouponNotFound;
            }

            if (result.Response!.CartItems.Any())
            {
                for (int i = 0; i < result.Response!.CartItems.Count; i++)
                {
                    var item = result.Response!.CartItems[i];

                    if (item.Promotion is not null)
                    {
                        continue;
                    }
                    else
                    {
                        ShoppingCartItem updatedItem;

                        switch (couponDiscount.PromotionCoupon.PromotionCouponPromotionEventType)
                        {
                            case PromotionEventTypeEnum.PromotionCoupon:
                                updatedItem = HandleCouponPromotion(item, couponDiscount);
                                result.Response!.CartItems[i] = updatedItem;
                                break;
                        }
                    }
                }
            }
        }

        GetBasketResponse response = MapToResponse(result.Response);

        return response;
    }

    private ShoppingCartItem HandleCouponPromotion(ShoppingCartItem item, CouponResponse couponDiscount)
    {
        decimal promotionDiscountUnitPrice = 0;
        decimal promotionFinalPrice = 0;
        int promotionAppliedProductCount = 0;
        int remainAvailableQuantityCoupon = 0;

        decimal subTotalAmount = item.Quantity * item.ProductUnitPrice;

        if (couponDiscount is not null)
        {
            remainAvailableQuantityCoupon = item.Quantity - (int)couponDiscount.PromotionCoupon.PromotionCouponAvailableQuantity!;

            if (remainAvailableQuantityCoupon < 0)
            {
                if ((int)couponDiscount.PromotionCoupon.PromotionCouponDiscountType == DiscountType.PERCENTAGE.Value)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!);
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    promotionAppliedProductCount = item.Quantity;
                }
                else if ((int)couponDiscount.PromotionCoupon.PromotionCouponDiscountType == DiscountType.FIXED.Value)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!;
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    promotionAppliedProductCount = item.Quantity;
                }
            }
            else
            {
                promotionDiscountUnitPrice = item.ProductUnitPrice;
                promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                promotionAppliedProductCount = item.Quantity - remainAvailableQuantityCoupon;
            }

            subTotalAmount = promotionFinalPrice;
        }
        else
        {
            item.Promotion = null;

            return item;
        }

        var promotion = Promotion.Create(promotionIdOrCode: couponDiscount.PromotionCoupon.PromotionCouponId,
                                         promotionEventType: couponDiscount.PromotionCoupon.PromotionCouponPromotionEventType.ToString(),
                                         promotionTitle: couponDiscount.PromotionCoupon.PromotionCouponTitle,
                                         promotionDiscountType: couponDiscount.PromotionCoupon.PromotionCouponDiscountType.ToString(),
                                         promotionDiscountValue: (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!,
                                         promotionDiscountUnitPrice: promotionDiscountUnitPrice,
                                         promotionAppliedProductCount: promotionAppliedProductCount,
                                         promotionFinalPrice: promotionFinalPrice);

        item.Promotion = promotion;

        item.SubTotalAmount = subTotalAmount;

        return item;
    }

    private GetBasketResponse MapToResponse(ShoppingCart? response)
    {
        var data = response!;

        var cartItems = new List<CartItemResponse>();

        List<CartItemResponse> cartItemsResponse = data.CartItems.Select(item =>
        {
            var promotion = item.Promotion == null ? null : new PromotionResponse()
            {
                PromotionIdOrCode = item.Promotion.PromotionIdOrCode,
                PromotionEventType = item.Promotion.PromotionEventType.ToString(),
                PromotionTitle = item.Promotion.PromotionTitle,
                PromotionDiscountType = item.Promotion.PromotionDiscountType.ToString(),
                PromotionDiscountValue = (decimal)item.Promotion.PromotionDiscountValue!,
                PromotionDiscountUnitPrice = (decimal)item.Promotion.PromotionDiscountUnitPrice!,
                PromotionAppliedProductCount = (int)item.Promotion.PromotionAppliedProductCount!,
                PromotionFinalPrice = (decimal)item.Promotion.PromotionFinalPrice!
            };

            return new CartItemResponse()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductColorName = item.ProductColorName,
                ProductUnitPrice = item.ProductUnitPrice,
                ProductNameTag = item.ProductNameTag,
                ProductImage = item.ProductImage,
                ProductSlug = item.ProductSlug,
                Quantity = item.Quantity,
                SubTotalAmount = (decimal)item.SubTotalAmount!,
                Promotion = promotion,
                OrderIndex = item.OrderIndex
            };
        }).ToList();

        return new GetBasketResponse()
        {
            UserEmail = data.UserEmail,
            CartItems = cartItemsResponse,
            TotalAmount = data.TotalAmount,
        };
    }
}