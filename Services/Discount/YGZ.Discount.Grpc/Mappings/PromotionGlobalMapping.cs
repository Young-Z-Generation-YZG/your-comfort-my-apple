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
            .Map(dest => dest.Title, src => src.PromotionGlobalModel.PromotionGlobalTitle)
            .Map(dest => dest.Description, src => src.PromotionGlobalModel.PromotionGlobalDescription)
            .Map(dest => dest.PromotionGlobalType, src => PromotionGlobalType.FromValue((int)src.PromotionGlobalModel.PromotionGlobalType))
            .Map(dest => dest.PromotionEventId, src => src.PromotionGlobalModel.PromotionGlobalEventId);
    }
}
