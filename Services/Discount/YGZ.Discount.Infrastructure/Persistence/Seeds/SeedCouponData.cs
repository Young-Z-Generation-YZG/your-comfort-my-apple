using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Seeds;

public static class SeedCouponData
{
    public static IEnumerable<Coupon> Coupons
    {
        get
        {
            return new List<Coupon>
            {
                Coupon.Create(
                    couponId: CouponId.Of("550e8400-e29b-41d4-a716-446655440000"),
                    userId: null,
                    code: Code.Of("SUMMER2025"),
                    title: "Summer 2025",
                    description: "Promotion coupon discount 10% for cart in summer 2025",
                    productClassification: EProductClassification.IPHONE,
                    promotionType: EPromotionType.COUPON,
                    discountType: EDiscountType.PERCENTAGE,
                    discountValue: 0.1,
                    maxDiscountAmount: null,
                    stock: 10,
                    discountState: EDiscountState.ACTIVE,
                    expiredDate: new DateTime(2026, 12, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(-7),
                    updatedBy: "SYSTEM"
                ),
                Coupon.Create(
                    couponId: CouponId.Of("550e8400-e29b-41d4-a716-446655440001"),
                    userId: null,
                    code: Code.Of("SUMMER2025IA"),
                    title: "Summer 2025",
                    description: "Promotion coupon discount 10% for cart in summer 2025",
                    productClassification: EProductClassification.IPHONE,
                    promotionType: EPromotionType.COUPON,
                    discountType: EDiscountType.PERCENTAGE,
                    discountValue: 0.1,
                    maxDiscountAmount: null,
                    stock: 10,
                    discountState: EDiscountState.INACTIVE,
                    expiredDate: new DateTime(2026, 12, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(-7),
                    updatedBy: "SYSTEM"
                ),
                Coupon.Create(
                    couponId: CouponId.Of("550e8400-e29b-41d4-a716-446655440002"),
                    userId: null,
                    code: Code.Of("SUMMER2026"),
                    title: "Summer 2026",
                    description: "Promotion coupon discount 10% for cart in summer 2026",
                    productClassification: EProductClassification.IPHONE,
                    promotionType: EPromotionType.COUPON,
                    discountType: EDiscountType.PERCENTAGE,
                    discountValue: 0.1,
                    maxDiscountAmount: 200,
                    stock: 10,
                    discountState: EDiscountState.INACTIVE,
                    expiredDate: new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(-7),
                    updatedBy: "SYSTEM"
                ),
                Coupon.Create(
                    couponId: CouponId.Of("550e8400-e29b-41d4-a716-446655440003"),
                    userId: null,
                    code: Code.Of("SUMMER2024"),
                    title: "Summer 2024",
                    description: "Promotion coupon discount 10% for cart in summer 2024",
                    productClassification: EProductClassification.IPHONE,
                    promotionType: EPromotionType.COUPON,
                    discountType: EDiscountType.PERCENTAGE,
                    discountValue: 0.1,
                    maxDiscountAmount: null,
                    stock: 10,
                    discountState: EDiscountState.ACTIVE,
                    expiredDate: new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(-7),
                    updatedBy: "SYSTEM"
                ),
                Coupon.Create(
                    couponId: CouponId.Of("550e8400-e29b-41d4-a716-446655440004"),
                    userId: "7ecf88f8-3e55-40cc-92d0-5d3a5a5e228f",
                    code: Code.Of("USER_PROMO"),
                    title: "User Promo Code",
                    description: "Promotion coupon discount 5% for user",
                    productClassification: EProductClassification.IPHONE,
                    promotionType: EPromotionType.COUPON,
                    discountType: EDiscountType.PERCENTAGE,
                    discountValue: 0.05,
                    maxDiscountAmount: null,
                    stock: 5,
                    discountState: EDiscountState.ACTIVE,
                    expiredDate: new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc).AddHours(-7),
                    updatedBy: "SYSTEM"
                )
            };
        }
    }
}
