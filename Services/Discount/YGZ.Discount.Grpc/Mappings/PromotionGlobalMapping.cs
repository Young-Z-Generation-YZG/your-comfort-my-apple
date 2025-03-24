using Mapster;
using YGZ.Discount.Application.Promotions.Commands.CreatePromotionGlobal;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionGlobalMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<PromotionGlobalModelRequest, CreatePromotionGlobalCommand>()
            .Map(dest => dest.Title, src => src.PromotionGlobalModel.Title)
            .Map(dest => dest.Description, src => src.PromotionGlobalModel.Description)
            .Map(dest => dest.PromotionGlobalType, src => PromotionGlobalType.FromValue((int)src.PromotionGlobalModel.GlobalType))
            .Map(dest => dest.PromotionEventId, src => src.PromotionGlobalModel.PromotionEventId);
    }
}
