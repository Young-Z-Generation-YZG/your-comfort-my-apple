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

        //config
        //    .NewConfig<CreatePromotionItemModelRequest, CreatePromotionItemCommand>()
        //    .Map(dest => dest.Title, src => src.PromotionItemModel.PromotionItemTitle)
        //    .Map(dest => dest.Description, src => src.PromotionItemModel.PromotionItemDescription)
        //    .Map(dest => dest.ProductNameTag, src => ProductNameTag.FromValue((int)src.PromotionItemModel.PromotionItemNameTag))
        //    .Map(dest => dest.State, src => DiscountState.FromValue((int)src.PromotionItemModel.PromotionItemState))
        //    .Map(dest => dest.Type, src => DiscountType.FromValue((int)src.PromotionItemModel.PromotionItemType))
        //    .Map(dest => dest.EndDiscountType, src => EndDiscountType.FromValue((int)src.PromotionItemModel.PromotionItemEndDiscountType))
        //    .Map(dest => dest.DiscountValue, src => src.PromotionItemModel.PromotionItemDiscountValue)
        //    .Map(dest => dest.ValidFrom, src => src.PromotionItemModel.PromotionItemValidFrom.ToDateTime())
        //    .Map(dest => dest.ValidTo, src => src.PromotionItemModel.PromotionItemValidTo.ToDateTime())
        //    .Map(dest => dest.AvailableQuantity, src => src.PromotionItemModel.PromotionItemAvailableQuantity)
        //    .Map(dest => dest.ProductModel, src => src.PromotionItemModel.PromotionItemProductModel)
        //    .Map(dest => dest.ProductStorage, src => src.PromotionItemModel.PromotionItemProductStorage)
        //    .Map(dest => dest.ProductSlug, src => src.PromotionItemModel.PromotionItemProductSlug)
        //    .Map(dest => dest.ProductImage, src => src.PromotionItemModel.PromotionItemProductImage);
    }
}
