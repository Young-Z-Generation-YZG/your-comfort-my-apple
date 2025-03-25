using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
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

        config.NewConfig<PromotionEventResponse, PromotionEventModel>()
            .Map(dest => dest.PromotionEventId, src => src.PromotionEventId)
            .Map(dest => dest.PromotionEventTitle, src => src.PromotionEventTitle)
            .Map(dest => dest.PromotionEventDescription, src => src.PromotionEventDescription)
            .Map(dest => dest.PromotionEventState, src => src.PromotionEventState)
            .Map(dest => dest.PromotionEventValidFrom, src => src.PromotionEventValidFrom)
            .Map(dest => dest.PromotionEventValidTo, src => src.PromotionEventValidTo);

        //config.NewConfig<PromotionProductResponse, PromotionProductModel>()
        //    .Map(dest => dest.ProductId, src => src.ProductId)
        //    .Map(dest => dest.ProductColorName, src => src.ProductColorName)
        //    .Map(dest => dest.ProductStorage, src => src.ProductStorage)
        //    .Map(dest => dest.ProductSlug, src => src.ProductSlug)
        //    .Map(dest => dest.ProductImage, src => src.ProductImage)
        //    .Map(dest => dest.DiscountPercent, src => src.DiscountPercent);

        //config.NewConfig<PromotionCategoryResponse, PromotionCategoryModel>()
        //    .Map(dest => dest.CategoryId, src => src.CategoryId)
        //    .Map(dest => dest.CategoryName, src => src.CategoryName)
        //    .Map(dest => dest.CategorySlug, src => src.CategorySlug)
        //    .Map(dest => dest.DiscountPercent, src => src.DiscountPercent);


    }
}