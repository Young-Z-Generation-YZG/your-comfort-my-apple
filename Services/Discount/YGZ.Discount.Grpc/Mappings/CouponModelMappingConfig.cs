using Google.Protobuf.WellKnownTypes;
using Mapster;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class CouponModelMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<Coupon, CouponModel>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Type, src => MapTypeEnum(src.Type.Name))
            .Map(dest => dest.State, src => MapStateEnum(src.State.Name))
            .Map(dest => dest.DiscountValue, src => src.DiscountValue)
            .Map(dest => dest.MinPurchaseAmount, src => src.MinPurchaseAmount.HasValue ? src.MinPurchaseAmount : null)
            .Map(dest => dest.MaxDiscountAmount, src => src.MaxDiscountAmount.HasValue ? src.MaxDiscountAmount : null)
            .Map(dest => dest.ValidFrom, src => src.ValidFrom.HasValue ? Timestamp.FromDateTime(src.ValidFrom.Value.ToUniversalTime()) : null)
            .Map(dest => dest.ValidTo, src => src.ValidTo.HasValue ? Timestamp.FromDateTime(src.ValidTo.Value.ToUniversalTime()) : null)
            .Map(dest => dest.AvailableQuantity, src => src.AvailableQuantity);
    }

    private TypeEnum MapTypeEnum(string typeName)
    {
        return typeName switch
        {
            "PERCENT" => TypeEnum.Percent,
            "FIXED" => TypeEnum.Fixed,
            _ => TypeEnum.DiscountTypeEnumUnknown
        };
    }

    private StateEnum MapStateEnum(string stateName)
    {
        return stateName switch
        {
            "ACTIVE" => StateEnum.Active,
            "INACTIVE" => StateEnum.Inactive,
            "EXPIRED" => StateEnum.Expired,
            _ => StateEnum.DiscountStateEnumUnknown
        };
    }
}