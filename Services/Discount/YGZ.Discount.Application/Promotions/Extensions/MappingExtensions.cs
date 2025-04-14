

using YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;
using YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;
using YGZ.Discount.Domain.PromotionEvent;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;
using YGZ.Discount.Domain.PromotionItem;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Application.PromotionCoupons.Extensions;

public static class MappingExtensions
{
    public static PromotionEvent ToEntity(this CreatePromotionEventCommand request)
    {
        return PromotionEvent.Create(
                id: PromotionEventId.Create(),
                title: request.Title,
                description: request.Description,
                discountState: request.State,
                validFrom: request.ValidFrom,
                validTo: request.ValidTo);
    }

    public static PromotionGlobal ToEntity(this CreatePromotionGlobalCommand request)
    {
        return PromotionGlobal.Create(
                id: PromotionGlobalId.Create(),
                title: request.Title,
                description: request.Description,
                promotionGlobalType: request.PromotionGlobalType,
                promotionEventId: PromotionEventId.Of(request.PromotionEventId));
    }

    public static PromotionProduct ToEntity(this PromotionProductCommand request)
    {
        return PromotionProduct.Create(id: ProductId.Of(request.ProductSlug),
                                       productSlug: request.ProductSlug,
                                       productImage: request.ProductImage,
                                       discountType: request.DiscountType,
                                       discountValue: request.DiscountValue,
                                       promotionGlobalId: PromotionGlobalId.Of(request.PromotionGlobalId));
    }

    public static PromotionItem ToEntity(this CreatePromotionItemCommand request)
    {
        return PromotionItem.Create(promotionItemId: PromotionItemId.Create(),
                                    productId: ProductId.Of(request.ProductId),
                                    title: request.Title,
                                    description: request.Description,
                                    discountState: request.DiscountState,
                                    discountType: request.DiscountType,
                                    endDiscountType: request.EndDiscountType,
                                    discountValue: request.DiscountValue,
                                    nameTag: request.ProductNameTag,
                                    validFrom: request.ValidFrom,
                                    validTo: request.ValidTo,
                                    availableQuantity: request.AvailableQuantity,
                                    productImage: request.ProductImage,
                                    productSlug: request.ProductSlug);
    }

    public static PromotionCategory ToEntity(this PromotionCategoryCommand request)
    {
        return PromotionCategory.Create(id: CategoryId.Of(request.CategoryId),
            categoryName: request.CategoryName,
            categorySlug: request.CategorySlug,
            discountType: request.DiscountType,
            discountValue: request.DiscountType,
            promotionGlobalId: PromotionGlobalId.Of(request.PromotionGlobalId));
    }
}
