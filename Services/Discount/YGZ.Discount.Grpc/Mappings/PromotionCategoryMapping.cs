using Mapster;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionCategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<PromotionCategoryModel, PromotionCategoryCommand>()
            .Map(dest => dest.CategoryId, src => src.CategoryId)
            .Map(dest => dest.CategoryName, src => src.CategoryName)
            .Map(dest => dest.CategorySlug, src => src.CategorySlug)
            .Map(dest => dest.PromotionGlobalId, src => src.PromotionGlobalId);
    }
}
