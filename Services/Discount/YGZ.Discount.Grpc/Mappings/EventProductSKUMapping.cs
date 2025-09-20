using Mapster;
using YGZ.Discount.Application.Events.Commands.AddEventProductSKUs;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class EventProductSKUMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Map from gRPC EventProductSKUModel to EventProductSKUCommand
        config.NewConfig<EventProductSKUModel, EventProductSKUCommand>()
            .Map(dest => dest.EventId, src => src.EventId)
            .Map(dest => dest.SkuId, src => src.SkuId)
            .Map(dest => dest.ModelId, src => src.ModelId)
            .Map(dest => dest.ModelName, src => src.ModelName)
            .Map(dest => dest.StorageName, src => src.StorageName)
            .Map(dest => dest.ColorHaxCode, src => src.ColorHexCode)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl)
            .Map(dest => dest.ProductType, src => ConvertProductTypeEnum(src.ProductType))
            .Map(dest => dest.DiscountType, src => ConvertDiscountTypeEnum(src.DiscountType))
            .Map(dest => dest.DiscountValue, src => (decimal)src.DiscountValue.Value)
            .Map(dest => dest.OriginalPrice, src => (decimal)src.OriginalPrice.Value)
            .Map(dest => dest.Stock, src => src.Stock.Value);

        // Map from gRPC AddEventProductSKUsRequest to AddEventProductSKUsCommand
        config.NewConfig<AddEventProductSKUsRequest, AddEventProductSKUsCommand>()
            .Map(dest => dest.EventProductSKUs, src => src.EventProductSkus);
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
