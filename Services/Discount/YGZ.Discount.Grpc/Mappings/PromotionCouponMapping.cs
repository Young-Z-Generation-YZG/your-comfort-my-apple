using Google.Protobuf.WellKnownTypes;
using Mapster;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Application.Coupons.Commands.DeleteCoupon;
using YGZ.Discount.Application.Coupons.Commands.UpdateCoupon;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Discount.Grpc.Mappings;

public class CouponModelMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<CreatePromotionCouponModel, CreatePromotionCouponCommand>()
            .Map(dest => dest.Title, src => src.PromotionCouponModel.PromotionCouponTitle)
            .Map(dest => dest.Code, src => src.PromotionCouponModel.PromotionCouponCode)
            .Map(dest => dest.Description, src => src.PromotionCouponModel.PromotionCouponDescription)
            .Map(dest => dest.ProductNameTag, src => ProductNameTag.FromValue((int)src.PromotionCouponModel.PromotionCouponProductNameTag))
            .Map(dest => dest.PromotionEventType, src => PromotionEventType.FromValue((int)src.PromotionCouponModel.PromotionCouponPromotionEventType))
            .Map(dest => dest.DiscountState, src => DiscountState.FromValue((int)src.PromotionCouponModel.PromotionCouponDiscountState))
            .Map(dest => dest.DiscountType, src => DiscountType.FromValue((int)src.PromotionCouponModel.PromotionCouponDiscountType))
            .Map(dest => dest.DiscountValue, src => src.PromotionCouponModel.PromotionCouponDiscountValue)
            .Map(dest => dest.MaxDiscountAmount, src => src.PromotionCouponModel.PromotionCouponMaxDiscountAmount)
            .Map(dest => dest.ValidFrom, src => src.PromotionCouponModel.PromotionCouponValidFrom.ToDateTime())
            .Map(dest => dest.ValidTo, src => src.PromotionCouponModel.PromotionCouponValidTo.ToDateTime())
            .Map(dest => dest.AvailableQuantity, src => src.PromotionCouponModel.PromotionCouponAvailableQuantity);

        config
            .NewConfig<UpdateDiscountCouponRequest, UpdateCouponCommand>()
            .Map(dest => dest.Id, src => src.PromotionCouponModel.PromotionCouponId)
            .Map(dest => dest.Code, src => src.PromotionCouponModel.PromotionCouponCode)
            .Map(dest => dest.Title, src => src.PromotionCouponModel.PromotionCouponCode)
            .Map(dest => dest.Description, src => src.PromotionCouponModel.PromotionCouponDescription)
            .Map(dest => dest.ProductNameTag, src => ProductNameTag.FromValue((int)src.PromotionCouponModel.PromotionCouponProductNameTag))
            .Map(dest => dest.DiscountState, src => DiscountState.FromValue((int)src.PromotionCouponModel.PromotionCouponDiscountState))
            .Map(dest => dest.DiscountType, src => DiscountType.FromValue((int)src.PromotionCouponModel.PromotionCouponDiscountType))
            .Map(dest => dest.DiscountValue, src => src.PromotionCouponModel.PromotionCouponDiscountValue)
            .Map(dest => dest.MaxDiscountAmount, src => src.PromotionCouponModel.PromotionCouponMaxDiscountAmount)
            .Map(dest => dest.ValidFrom, src => src.PromotionCouponModel.PromotionCouponValidFrom.ToDateTime())
            .Map(dest => dest.ValidTo, src => src.PromotionCouponModel.PromotionCouponValidTo.ToDateTime())
            .Map(dest => dest.AvailableQuantity, src => src.PromotionCouponModel.PromotionCouponAvailableQuantity);


        config
            .NewConfig<DeleteDiscountCouponRequest, DeleteCouponCommand>()
            .Map(dest => dest.CouponId, src => src.CouponId);

        //config.NewConfig<Coupon, CouponModel>()
        //    .Map(dest => dest.Title, src => src.Title)
        //    .Map(dest => dest.Description, src => src.Description)
        //    .Map(dest => dest.State, src => MapStateEnum(src.State.Name))
        //    .Map(dest => dest.ProductNameTag, src => MapNameTagEnum(src.ProductNameTag.Name))
        //    .Map(dest => dest.Type, src => MapTypeEnum(src.Type.Name))
        //    .Map(dest => dest.DiscountValue, src => src.DiscountValue)
        //    .Map(dest => dest.MaxDiscountAmount, src => src.MaxDiscountAmount.HasValue ? src.MaxDiscountAmount : null)
        //    .Map(dest => dest.ValidFrom, src => src.ValidFrom.HasValue ? Timestamp.FromDateTime(src.ValidFrom.Value.ToUniversalTime()) : null)
        //    .Map(dest => dest.ValidTo, src => src.ValidTo.HasValue ? Timestamp.FromDateTime(src.ValidTo.Value.ToUniversalTime()) : null)
        //    .Map(dest => dest.AvailableQuantity, src => src.AvailableQuantity);
    }
}