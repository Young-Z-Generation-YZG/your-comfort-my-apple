using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings.Responses;

public class PromotionGlobalResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<PromotionGlobalEventResponse, PromtionEventResponse>()
            .Map(dest => dest.PromotionEvent, src => src.promotionEvent)
            .Map(dest => dest.PromotionProducs, src => src.PromotionProducts)
            .Map(dest => dest.PromotionCategories, src => src.PromotionCategories);
    }
}
