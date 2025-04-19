using Google.Protobuf.WellKnownTypes;
using Mapster;
using YamlDotNet.Core.Tokens;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
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
            .Map(dest => dest.Title, src => src.PromotionItemModel.PromotionItemTitle)
            .Map(dest => dest.Description, src => src.PromotionItemModel.PromotionItemDescription)
            .Map(dest => dest.ProductNameTag, src => ProductNameTag.FromValue((int)src.PromotionItemModel.PromotionItemNameTag))
            .Map(dest => dest.DiscountState, src => DiscountState.FromValue((int)src.PromotionItemModel.PromotionItemDiscountState))
            .Map(dest => dest.DiscountType, src => DiscountType.FromValue((int)src.PromotionItemModel.PromotionItemDiscountType))
            .Map(dest => dest.EndDiscountType, src => EndDiscountType.FromValue((int)src.PromotionItemModel.PromotionItemEndDiscountType))
            .Map(dest => dest.DiscountValue, src => src.PromotionItemModel.PromotionItemDiscountValue)
            .Map(dest => dest.ValidFrom, src => src.PromotionItemModel.PromotionItemValidFrom.ToDateTime())
            .Map(dest => dest.ValidTo, src => src.PromotionItemModel.PromotionItemValidTo.ToDateTime())
            .Map(dest => dest.AvailableQuantity, src => src.PromotionItemModel.PromotionItemAvailableQuantity)
            .Map(dest => dest.ProductId, src => src.PromotionItemModel.PromotionItemProductId)
            .Map(dest => dest.ProductSlug, src => src.PromotionItemModel.PromotionItemProductSlug)
            .Map(dest => dest.ProductImage, src => src.PromotionItemModel.PromotionItemProductImage);

        config.NewConfig<PromotionItemResponse, PromotionItemModel>()
            .Map(dest => dest.PromotionItemProductId, src => src.ProductId)
            .Map(dest => dest.PromotionItemTitle, src => src.Title)
            .Map(dest => dest.PromotionItemDescription, src => src.Description)
            .Map(dest => dest.PromotionItemNameTag, src => (ProductNameTagEnum)ProductNameTag.FromName(src.ProductNameTag, false).Value)
            .Map(dest => dest.PromotionItemPromotionEventType, src => (PromotionEventTypeEnum)PromotionEventType.FromName(src.PromotionEventType, false).Value)
            .Map(dest => dest.PromotionItemDiscountState, src => (DiscountStateEnum)DiscountState.FromName(src.DiscountState, false).Value)
            .Map(dest => dest.PromotionItemDiscountType, src => (DiscountTypeEnum)DiscountType.FromName(src.DiscountType, false).Value)
            .Map(dest => dest.PromotionItemEndDiscountType, src => (EndDiscountEnum)EndDiscountType.FromName(src.EndDiscountType, false).Value)
            .Map(dest => dest.PromotionItemDiscountValue, src => src.DiscountValue)
            .Map(dest => dest.PromotionItemValidFrom, src => src.ValidFrom.HasValue ? src.ValidFrom.Value.ToTimestamp() : null)
            .Map(dest => dest.PromotionItemValidTo, src => src.ValidTo.HasValue ? src.ValidTo.Value.ToTimestamp() : null)
            .Map(dest => dest.PromotionItemAvailableQuantity, src => src.AvailableQuantity)
            .Map(dest => dest.PromotionItemProductImage, src => src.PromotionItemProductImage)
            .Map(dest => dest.PromotionItemProductSlug, src => src.PromotionItemProductSlug);
    }
}


