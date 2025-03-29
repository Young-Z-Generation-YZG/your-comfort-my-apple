

using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.PromotionEvent.Entities;

namespace YGZ.Discount.Application.Promotions.Mappings;

public class PromotionProductMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<PromotionCategory, PromotionCategoryResponse>()
            .Map(dest => dest.CategoryId, src => src.Id.Value.ToString())
            .Map(dest => dest.CategoryName, src => src.CategoryName)
            .Map(dest => dest.CategorySlug, src => src.CategorySlug)
            .Map(dest => dest.DiscountType, src => src.DiscountType.Name)
            .Map(dest => dest.DiscountValue, src => src.DiscountValue)
            .Map(dest => dest.PromotionGlobalId, src => src.PromotionGlobalId.Value.ToString());
    }
}
