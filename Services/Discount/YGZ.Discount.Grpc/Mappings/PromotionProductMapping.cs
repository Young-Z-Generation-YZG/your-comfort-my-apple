using Mapster;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionProduct;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionProductMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<PromotionProductModel, PromotionProductCommand>()
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.ProductColorName, src => src.ProductColorName)
            .Map(dest => dest.ProductStorage, src => src.ProductStorage)
            .Map(dest => dest.ProductSlug, src => src.ProductSlug)
            .Map(dest => dest.ProductImage, src => src.ProductImage)
            .Map(dest => dest.PromotionGlobalId, src => src.PromotionGlobalId);
    }
}
