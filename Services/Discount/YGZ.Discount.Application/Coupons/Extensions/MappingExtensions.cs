

using YGZ.Discount.Application.Coupons.Commands.CreateCoupon;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Application.Coupons.Extensions;

public static class MappingExtensions
{
    public static Coupon ToEntity(this CreatePromotionCouponCommand request)
    {
        return Coupon.Create(
                couponId: CouponId.Create(),
                code: string.IsNullOrEmpty(request.Code) ? Code.Create() : Code.Of(request.Code), // Ensure code is not null
                title: request.Title,
                description: request.Description,
                nameTag: request.ProductNameTag,
                promotionEventType: request.PromotionEventType,
                discountState: request.DiscountState,
                discountType: request.DiscountType,
                discountValue: request.DiscountValue,
                maxDiscountAmount: request.MaxDiscountAmount,
                validFrom: request.ValidFrom,
                validTo: request.ValidTo,
                availableQuantity: request.AvailableQuantity
            );
    }
}
