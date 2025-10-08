using Mapster;
using YGZ.Discount.Application.Events.Commands.AddEventItem;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class EventItemMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map from gRPC Model to ModelCommand
        config.NewConfig<ModelValueObject, ModelCommand>();

        // Map from gRPC Color to ColorCommand
        config.NewConfig<ColorValueObject, ColorCommand>();

        // Map from gRPC Storage to StorageCommand
        config.NewConfig<StorageValueObject, StorageCommand>();

        // Map from gRPC EventItem to EventItemCommand
        config.NewConfig<EventItemModel, EventItemCommand>()
            .Map(dest => dest.ProductType, src => ConvertProductTypeEnum(src.ProductType))
            .Map(dest => dest.DiscountType, src => ConvertDiscountTypeEnum(src.DiscountType));

        // Map from gRPC AddEventItemsRequest to AddEventItemsCommand
        config.NewConfig<AddEventItemsRequest, AddEventItemsCommand>()
            .Map(dest => dest.EventId, src => src.EventId)
            .Map(dest => dest.EventItems, src => src.EventItems);
    }

    private static string ConvertProductTypeEnum(EProductType productType)
    {
        return productType switch
        {
            EProductType.ProductTypeUnknown => "UNKNOWN",
            EProductType.ProductTypeIphone => "IPHONE",
            EProductType.ProductTypeIpad => "IPAD",
            EProductType.ProductTypeMacbook => "MACBOOK",
            EProductType.ProductTypeWatch => "WATCH",
            EProductType.ProductTypeHeadphone => "HEADPHONE",
            EProductType.ProductTypeAccessory => "ACCESSORY",
            _ => "UNKNOWN"
        };
    }

    private static string ConvertDiscountTypeEnum(EDiscountType discountType)
    {
        return discountType switch
        {
            EDiscountType.DiscountTypeUnknown => "UNKNOWN",
            EDiscountType.DiscountTypePercentage => "PERCENTAGE",
            EDiscountType.DiscountTypeFixed => "FIXED",
            _ => "UNKNOWN"
        };
    }
}
