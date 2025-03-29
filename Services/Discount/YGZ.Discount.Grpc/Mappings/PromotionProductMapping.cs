using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionProductMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<PromotionProductModel, PromotionProductCommand>()
            .Map(dest => dest.ProductId, src => src.PromotionProductId)
            .Map(dest => dest.ProductSlug, src => src.PromotionProductSlug)
            .Map(dest => dest.ProductImage, src => src.PromotionProductImage)
            .Map(dest => dest.DiscountType, src => DiscountType.FromValue((int)src.PromotionProductDiscountType))
            .Map(dest => dest.DiscountValue, src => src.PromotionProductDiscountValue)
            .Map(dest => dest.PromotionGlobalId, src => src.PromotionProductPromotionGlobalId);

        config
        .NewConfig<PromotionProductResponse, PromotionProductModel>()
        .Map(dest => dest.PromotionProductId, src => src.ProductId)
        .Map(dest => dest.PromotionProductSlug, src => src.ProductSlug)
        .Map(dest => dest.PromotionProductImage, src => src.ProductImage)
        .Map(dest => dest.PromotionProductDiscountType, src => src.DiscountType)
        .Map(dest => dest.PromotionProductDiscountValue, src => src.DiscountValue)
        .Map(dest => dest.PromotionProductPromotionGlobalId, src => src.PromotionGlobalId);

    }
}