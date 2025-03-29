

using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.PromotionEvent.Entities;

namespace YGZ.Discount.Application.Promotions.Mappings;

public class PromotionCategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<PromotionProduct, PromotionProductResponse>()
            .Map(dest => dest.ProductId, src => src.Id.Value.ToString())
            .Map(dest => dest.ProductSlug, src => src.ProductSlug)
            .Map(dest => dest.ProductImage, src => src.ProductImage)
            .Map(dest => dest.DiscountType, src => src.DiscountType.Name)
            .Map(dest => dest.DiscountValue, src => src.DiscountValue)
            .Map(dest => dest.PromotionGlobalId, src => src.PromotionGlobalId.Value.ToString());
    }
}
