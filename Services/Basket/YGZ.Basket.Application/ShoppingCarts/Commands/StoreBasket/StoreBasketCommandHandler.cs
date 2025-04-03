
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket.Extensions;
using YGZ.Basket.Domain.Core.Enums;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreBasket;

public class StoreBasketCommandHandler : ICommandHandler<StoreBasketCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;
    private readonly ILogger<StoreBasketCommandHandler> _logger;
    private readonly IUserContext _userContext;

    public StoreBasketCommandHandler(IBasketRepository basketRepository,
                                     ILogger<StoreBasketCommandHandler> logger,
                                     IUserContext userContext,
                                     DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _userContext = userContext;
        _discountProtoServiceClient = discountProtoServiceClient;
    }

    public async Task<Result<bool>> Handle(StoreBasketCommand request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();

        ShoppingCart shoppingCart = request.ToEntity("lov3rinve146@gmail.com");

        if (shoppingCart.CartItems.Any())
        {
            for (int i = 0; i < shoppingCart.CartItems.Count; i++)
            {
                var item = shoppingCart.CartItems[i];
                var promotion = item.Promotion;
                if (promotion is null)
                {
                    continue;
                }
                else
                {
                    ShoppingCartItem updatedItem;

                    switch (promotion.PromotionEventType)
                    {
                        case nameof(PromotionEvent.PROMOTION_ITEM):
                            updatedItem = await HandleItemPromotion(item);
                            shoppingCart.CartItems[i] = updatedItem; // Safe to update here
                            break;
                        case nameof(PromotionEvent.PROMOTION_EVENT):
                            updatedItem = await HandleEventPromotion(item);
                            shoppingCart.CartItems[i] = updatedItem; // Safe to update here 
                            break;
                    }
                }
            }

            var result = await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error;
            }

        }

        return true;
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
        } else
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

            if(promotionEvent is null)
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

                if(discountType == DiscountTypeEnum.Percentage)
                {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (item.ProductUnitPrice * (decimal)promotionProduct.PromotionProductDiscountValue!);
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    discountType = DiscountTypeEnum.Percentage;
                    discountValue = (decimal)promotionProduct.PromotionProductDiscountValue!;
                } else {
                    promotionDiscountUnitPrice = item.ProductUnitPrice - (decimal)promotionProduct.PromotionProductDiscountValue!;
                    promotionFinalPrice = promotionDiscountUnitPrice * item.Quantity;
                    discountType = DiscountTypeEnum.Fixed;
                    discountValue = (decimal)promotionProduct.PromotionProductDiscountValue!;
                }
                
                promotionAppliedProductCount = item.Quantity;
            }

            if(promotionCategory is not null)
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

            if(promotionDiscountUnitPrice == -1)
            {
                item.Promotion = null;

                return item;
            }

            subTotalAmount = promotionFinalPrice;
            item.Promotion!.PromotionTitle = promotionEvent.PromotionEvent.PromotionEventTitle;
        } else
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
    
}