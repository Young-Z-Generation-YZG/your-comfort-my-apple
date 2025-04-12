
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

            if (couponDiscount.PromotionCoupon is null)
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
        } else
        {
            for (int i = 0; i < result.Response!.CartItems.Count; i++)
            {
                var item = result.Response!.CartItems[i];

                ShoppingCartItem updatedItem;

                if(item.Promotion is null)
                {
                    continue;
                }
                else
                {
                    switch (item.Promotion.PromotionEventType)
                    {
                        case nameof(PromotionEvent.PROMOTION_ITEM):
                            updatedItem = await HandleItemPromotion(item);
                            result.Response.CartItems[i] = updatedItem; // Safe to update here
                            break;
                        case nameof(PromotionEvent.PROMOTION_EVENT):
                            updatedItem = await HandleEventPromotion(item);
                            result.Response.CartItems[i] = updatedItem; // Safe to update here 
                            break;
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
        promotionFinalPrice = subTotalAmount;

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
                                         promotionEventType: PromotionEvent.FromValue((int)couponDiscount.PromotionCoupon.PromotionCouponPromotionEventType).Name,
                                         promotionTitle: couponDiscount.PromotionCoupon.PromotionCouponTitle,
                                         promotionDiscountType: DiscountType.FromValue((int)couponDiscount.PromotionCoupon.PromotionCouponDiscountType).Name,
                                         promotionDiscountValue: (decimal)couponDiscount.PromotionCoupon.PromotionCouponDiscountValue!,
                                         promotionDiscountUnitPrice: promotionDiscountUnitPrice,
                                         promotionAppliedProductCount: promotionAppliedProductCount,
                                         promotionFinalPrice: promotionFinalPrice);

        item.Promotion = promotion;

        item.SubTotalAmount = subTotalAmount;

        return item;
    }

    private async Task<ShoppingCartItem> HandleItemPromotion(ShoppingCartItem item)
    {
        decimal promotionDiscountUnitPrice = -1;
        decimal promotionFinalPrice = 0;
        int promotionAppliedProductCount = 0;
        DiscountTypeEnum discountType = DiscountTypeEnum.Percentage;
        decimal discountValue = 0;

        decimal subTotalAmount = item.Quantity * item.ProductUnitPrice;

        var promotionItem = await _discountProtoServiceClient.GetPromotionItemByIdAsync(new GetPromotionItemByIdRequest
        {
            PromotionId = item.Promotion!.PromotionIdOrCode
        });

        if (promotionItem is not null && item.ProductSlug == promotionItem.PromotionItemProductSlug)
        {
            discountType = promotionItem.PromotionItemDiscountType;
            discountValue = (decimal)promotionItem.PromotionItemDiscountValue!;

            if (discountType == DiscountTypeEnum.Percentage)
            {
                promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionItem.PromotionItemDiscountValue!);
                promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                discountType = DiscountTypeEnum.Percentage;
            }
            else
            {
                promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)promotionItem.PromotionItemDiscountValue!;
                promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                discountType = DiscountTypeEnum.Fixed;
            }

            promotionAppliedProductCount = item.Quantity;
            item.Promotion!.PromotionTitle = promotionItem.PromotionItemTitle;
        }
        else
        {
            item.Promotion = null;

            return item;
        }

        item.Promotion!.PromotionDiscountType = discountType.ToString().ToUpper();
        item.Promotion.PromotionDiscountValue = discountValue;
        item.Promotion!.PromotionDiscountUnitPrice = promotionDiscountUnitPrice;
        item.Promotion.PromotionAppliedProductCount = promotionAppliedProductCount;
        item.Promotion.PromotionFinalPrice = promotionFinalPrice;

        return item;
    }

    private async Task<ShoppingCartItem> HandleEventPromotion(ShoppingCartItem item)
    {
        decimal promotionDiscountUnitPrice = -1;
        decimal promotionFinalPrice = 0;
        int promotionAppliedProductCount = 0;
        DiscountTypeEnum discountType = DiscountTypeEnum.Percentage;
        decimal discountValue = 0;

        decimal subTotalAmount = item.Quantity * item.ProductUnitPrice;

        var promotionEvents = await _discountProtoServiceClient.GetPromotionEventAsync(new GetPromotionEventRequest { });

        if (promotionEvents is not null)
        {
            var promotionEvent = promotionEvents.PromotionEvents.FirstOrDefault(x => x.PromotionEvent.PromotionEventId == item.Promotion?.PromotionIdOrCode);

            if (promotionEvent is null)
            {
                return item;
            }

            // else
            List<PromotionProductModel> promotionProducts = new List<PromotionProductModel>();
            List<PromotionCategoryModel> promotionCategories = new List<PromotionCategoryModel>();

            promotionProducts = promotionEvent.PromotionProducts.ToList();
            promotionCategories = promotionEvent.PromotionCategories.ToList();

            var promotionProduct = promotionProducts.FirstOrDefault(pp => pp.PromotionProductId == item.ProductSlug);
            var promotionCategory = promotionCategories.FirstOrDefault(pc => pc.PromotionCategoryId == item.CategoryId);



            if (promotionProduct is not null && promotionProduct.PromotionProductSlug == item.ProductSlug)
            {
                discountType = promotionProduct.PromotionProductDiscountType;

                if (discountType == DiscountTypeEnum.Percentage)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionProduct.PromotionProductDiscountValue!);
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    discountType = DiscountTypeEnum.Percentage;
                    discountValue = (decimal)promotionProduct.PromotionProductDiscountValue!;
                }
                else
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)promotionProduct.PromotionProductDiscountValue!;
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    discountType = DiscountTypeEnum.Fixed;
                    discountValue = (decimal)promotionProduct.PromotionProductDiscountValue!;
                }

                promotionAppliedProductCount = item.Quantity;
            }

            if (promotionCategory is not null)
            {
                discountType = promotionCategory.PromotionCategoryDiscountType;

                if (discountType == DiscountTypeEnum.Percentage)
                {
                    decimal categoryDiscountPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionCategory.PromotionCategoryDiscountValue!);

                    if (promotionDiscountUnitPrice == -1 || categoryDiscountPrice < promotionDiscountUnitPrice)
                    {
                        promotionDiscountUnitPrice = categoryDiscountPrice;
                        promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                        discountType = DiscountTypeEnum.Percentage;
                        discountValue = (decimal)promotionCategory.PromotionCategoryDiscountValue!;
                    }
                }

                promotionAppliedProductCount = item.Quantity;
            }

            if (promotionDiscountUnitPrice == -1)
            {
                item.Promotion = null;

                return item;
            }

            subTotalAmount = promotionFinalPrice;
            item.Promotion!.PromotionTitle = promotionEvent.PromotionEvent.PromotionEventTitle;
        }
        else
        {
            item.Promotion = null;

            return item;
        }

        item.Promotion!.PromotionDiscountType = discountType.ToString().ToUpper();
        item.Promotion.PromotionDiscountValue = discountValue;
        item.Promotion!.PromotionDiscountUnitPrice = promotionDiscountUnitPrice;
        item.Promotion.PromotionAppliedProductCount = promotionAppliedProductCount;
        item.Promotion.PromotionFinalPrice = promotionFinalPrice;

        item.SubTotalAmount = subTotalAmount;

        return item;
    }

    private GetBasketResponse MapToResponse(ShoppingCart? response)
    {
        if (response is null)
        {
            return new GetBasketResponse()
            {
                UserEmail = string.Empty,
                CartItems = new List<CartItemResponse>(),
                TotalAmount = 0
            };
        }

        var cartItems = new List<CartItemResponse>();

        List<CartItemResponse> cartItemsResponse = response.CartItems.Select(item =>
        {
            var promotion = item.Promotion is null ? null : new PromotionResponse()
            {
                PromotionIdOrCode = item.Promotion.PromotionIdOrCode,
                PromotionEventType = item.Promotion.PromotionEventType,
                PromotionTitle = item.Promotion.PromotionTitle,
                PromotionDiscountType = item.Promotion.PromotionDiscountType,
                PromotionDiscountValue = (decimal)(item.Promotion.PromotionDiscountValue ?? 0),
                PromotionDiscountUnitPrice = (decimal)(item.Promotion.PromotionDiscountUnitPrice ?? 0),
                PromotionAppliedProductCount = (int)(item.Promotion.PromotionAppliedProductCount ?? 0),
                PromotionFinalPrice = (decimal)(item.Promotion.PromotionFinalPrice ?? 0)
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
                CategoryId = item.CategoryId,
                Quantity = item.Quantity,
                SubTotalAmount = item.SubTotalAmount ?? (decimal)(item.Quantity * item.ProductUnitPrice),
                Promotion = promotion ?? null,
                OrderIndex = item.OrderIndex
            };
        }).ToList();

        return new GetBasketResponse()
        {
            UserEmail = response.UserEmail,
            CartItems = cartItemsResponse,
            TotalAmount = response.TotalAmount,
        };
    }
}