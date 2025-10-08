using Mapster;

namespace YGZ.Discount.Grpc.Mappings.Responses;

public class CouponResponseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        //config
        //    .NewConfig<PromotionCouponResponse, CouponResponse>()
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponId, src => src.PromotionCouponId)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponTitle, src => src.PromotionCouponTitle)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponCode, src => src.PromotionCouponCode)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponDescription, src => src.PromotionCouponDescription)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponProductNameTag, src => (ProductNameTagEnum)ProductNameTag.FromName(src.PromotionCouponProductNameTag, false).Value)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponPromotionEventType, src => (PromotionEventTypeEnum)PromotionEventType.FromName(src.PromotionCouponPromotionEventType, false).Value)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponDiscountState, src => (DiscountStateEnum)DiscountState.FromName(src.PromotionCouponDiscountState, false).Value)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponDiscountType, src => (DiscountTypeEnum)DiscountType.FromName(src.PromotionCouponDiscountType, false).Value)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponDiscountValue, src => src.PromotionCouponDiscountValue)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponMaxDiscountAmount, src => src.PromotionCouponMaxDiscountAmount)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponValidFrom, src => src.PromotionCouponValidFrom.HasValue ? src.PromotionCouponValidFrom.Value.ToTimestamp() : null)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponValidTo, src => src.PromotionCouponValidTo.HasValue ? src.PromotionCouponValidTo.Value.ToTimestamp(): null)
        //    .Map(dest => dest.PromotionCoupon.PromotionCouponAvailableQuantity, src => src.PromotionCouponAvailableQuantity);
    }
}
