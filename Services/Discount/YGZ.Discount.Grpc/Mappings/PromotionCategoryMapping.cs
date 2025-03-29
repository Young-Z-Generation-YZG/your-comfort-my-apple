using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionCategory;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionCategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<PromotionCategoryModel, PromotionCategoryCommand>()
            .Map(dest => dest.CategoryId, src => src.PromotionCategoryId)
            .Map(dest => dest.CategoryName, src => src.PromotionCategoryName)
            .Map(dest => dest.CategorySlug, src => src.PromotionCategorySlug)
            .Map(dest => dest.DiscountType, src => DiscountState.FromValue((int)src.PromotionCategoryDiscountType))
            .Map(dest => dest.DiscountValue, src => src.PromotionCategoryDiscountValue)
            .Map(dest => dest.PromotionGlobalId, src => src.PromotionCategoryPromotionGlobalId);

        config
            .NewConfig<PromotionCategoryResponse, PromotionCategoryModel>()
            .Map(dest => dest.PromotionCategoryId, src => src.CategoryId)
            .Map(dest => dest.PromotionCategoryName, src => src.CategoryName)
            .Map(dest => dest.PromotionCategorySlug, src => src.CategorySlug)
            .Map(dest => dest.PromotionCategoryDiscountType, src => src.DiscountType)
            .Map(dest => dest.PromotionCategoryDiscountValue, src => src.DiscountValue)
            .Map(dest => dest.PromotionCategoryPromotionGlobalId, src => src.PromotionGlobalId);
    }
}
