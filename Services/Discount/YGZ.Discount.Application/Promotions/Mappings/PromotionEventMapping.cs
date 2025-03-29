using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.PromotionEvent;

namespace YGZ.Discount.Application.Promotions.Mappings;

public class PromotionEventMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<PromotionEvent, PromotionEventResponse>()
            .Map(dest => dest.PromotionEventId, src => src.Id.ToString())
            .Map(dest => dest.PromotionEventTitle, src => src.Title)
            .Map(dest => dest.PromotionEventDescription, src => src.Description)
            .Map(dest => dest.PromotionEventState, src => src.DiscountState.Name)
            .Map(dest => dest.PromotionEventValidFrom, src => src.ValidFrom)
            .Map(dest => dest.PromotionEventValidTo, src => src.ValidTo);
    }
}