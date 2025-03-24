

using YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;
using YGZ.Discount.Domain.PromotionEvent;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Application.PromotionCoupons.Extensions;

public static class MappingExtension
{
    public static PromotionEvent ToEntity(this CreatePromotionEventCommand request)
    {
        return PromotionEvent.Create(
                id: PromotionEventId.Create(),
                title: request.Title,
                description: request.Description,
                state: request.State,
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
        return PromotionProduct.Create(id: ProductId.Of(request.ProductId),
                                       productColorName: request.ProductColorName,
                                       productStorage: request.ProductStorage,
                                       productSlug: request.ProductSlug,
                                       productImage: request.ProductImage,
                                       discountPercentage: request.DiscountPercentage,
                                       promotionGlobalId: PromotionGlobalId.Of(request.PromotionGlobalId));
    }

    public static PromotionCategory ToEntity(this PromotionCategoryCommand request)
    {
        return PromotionCategory.Create(id: CategoryId.Of(request.CategoryId),
            categoryName: request.CategoryName,
            categorySlug: request.CategorySlug,
            discountPercentage: request.DiscountPercentage,
            promotionGlobalId: PromotionGlobalId.Of(request.PromotionGlobalId));
    }
}
