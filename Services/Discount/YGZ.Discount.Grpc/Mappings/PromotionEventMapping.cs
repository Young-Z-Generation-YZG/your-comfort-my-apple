
using Mapster;
using YGZ.Discount.Application.PromotionCoupons.Commands.CreatePromotionEvent;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionEventMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CreatePromotionEventModelRequest, CreatePromotionEventCommand>()
            .Map(dest => dest.Title, src => src.PromotionEventModel.Title)
            .Map(dest => dest.Description, src => src.PromotionEventModel.Description)
            .Map(dest => dest.State, src => DiscountState.FromValue((int)src.PromotionEventModel.State))
            .Map(dest => dest.ValidFrom, src => src.PromotionEventModel.ValidFrom.ToDateTime())
            .Map(dest => dest.ValidTo, src => src.PromotionEventModel.ValidTo.ToDateTime());
    }
}
