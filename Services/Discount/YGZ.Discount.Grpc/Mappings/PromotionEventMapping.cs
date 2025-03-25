
using Google.Protobuf.WellKnownTypes;
using Mapster;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
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
            .Map(dest => dest.Title, src => src.PromotionEventModel.PromotionEventTitle)
            .Map(dest => dest.Description, src => src.PromotionEventModel.PromotionEventDescription)
            .Map(dest => dest.State, src => DiscountState.FromValue((int)src.PromotionEventModel.PromotionEventState))
            .Map(dest => dest.ValidFrom, src => src.PromotionEventModel.PromotionEventValidFrom.ToDateTime())
            .Map(dest => dest.ValidTo, src => src.PromotionEventModel.PromotionEventValidTo.ToDateTime());

        config.NewConfig<PromotionEventResponse, PromotionEventModel>()
            .Map(dest => dest.PromotionEventId, src => src.PromotionEventId)
            .Map(dest => dest.PromotionEventTitle, src => src.PromotionEventTitle)
            .Map(dest => dest.PromotionEventDescription, src => src.PromotionEventDescription)
            .Map(dest => dest.PromotionEventState, src => DiscountState.FromName(src.PromotionEventState, false).Value)
            .Map(dest => dest.PromotionEventValidFrom, src => src.PromotionEventValidFrom.HasValue ? src.PromotionEventValidFrom.Value.ToTimestamp() : null)
            .Map(dest => dest.PromotionEventValidTo, src => src.PromotionEventValidTo.HasValue ? src.PromotionEventValidTo.Value.ToTimestamp() : null);
    }
}
