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
            .Map(dest => dest.CategoryType, src => ConvertProductTypeEnum(src.CategoryType))
            .Map(dest => dest.DiscountType, src => ConvertDiscountTypeEnum(src.DiscountType));

        // Map from gRPC AddEventItemsRequest to AddEventItemsCommand
        config.NewConfig<AddEventItemsRequest, AddEventItemsCommand>()
            .Map(dest => dest.EventId, src => src.EventId)
            .Map(dest => dest.EventItems, src => src.EventItems);
    }

    private static string ConvertProductTypeEnum(ECategoryTypeGrpc productClassification)
    {
        return productClassification switch
        {
            ECategoryTypeGrpc.CategoryTypeUnknown => "UNKNOWN",
            ECategoryTypeGrpc.CategoryTypeIphone => "IPHONE",
            ECategoryTypeGrpc.CategoryTypeIpad => "IPAD",
            ECategoryTypeGrpc.CategoryTypeMacbook => "MACBOOK",
            ECategoryTypeGrpc.CategoryTypeWatch => "WATCH",
            ECategoryTypeGrpc.CategoryTypeHeadphone => "HEADPHONE",
            ECategoryTypeGrpc.CategoryTypeAccessory => "ACCESSORY",
            _ => "UNKNOWN"
        };
    }

    private static string ConvertDiscountTypeEnum(EDiscountTypeGrpc discountType)
    {
        return discountType switch
        {
            EDiscountTypeGrpc.DiscountTypeUnknown => "UNKNOWN",
            EDiscountTypeGrpc.DiscountTypePercentage => "PERCENTAGE",
            EDiscountTypeGrpc.DiscountTypeFixed => "FIXED",
            _ => "UNKNOWN"
        };
    }
}
