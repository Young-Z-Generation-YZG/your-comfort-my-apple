using Mapster;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Coupons.Commands.CreatePromotionItem;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class PromotionItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config
            .NewConfig<CreatePromotionItemModelRequest, CreatePromotionItemCommand>()
            .Map(dest => dest.Title, src => src.PromotionItemModel.Title)
            .Map(dest => dest.Description, src => src.PromotionItemModel.Description)
            .Map(dest => dest.NameTag, src => NameTag.FromValue((int)src.PromotionItemModel.NameTag))
            .Map(dest => dest.State, src => DiscountState.FromValue((int)src.PromotionItemModel.State))
            .Map(dest => dest.Type, src => DiscountType.FromValue((int)src.PromotionItemModel.Type))
            .Map(dest => dest.DiscountValue, src => src.PromotionItemModel.DiscountValue)
            .Map(dest => dest.ValidFrom, src => src.PromotionItemModel.ValidFrom.ToDateTime())
            .Map(dest => dest.ValidTo, src => src.PromotionItemModel.ValidTo.ToDateTime())
            .Map(dest => dest.AvailableQuantity, src => src.PromotionItemModel.AvailableQuantity)
            .Map(dest => dest.ProductId, src => src.PromotionItemModel.ProductId)
            .Map(dest => dest.ProductSlug, src => src.PromotionItemModel.ProductSlug)
            .Map(dest => dest.ProductImage, src => src.PromotionItemModel.ProductImage);
    }
}
