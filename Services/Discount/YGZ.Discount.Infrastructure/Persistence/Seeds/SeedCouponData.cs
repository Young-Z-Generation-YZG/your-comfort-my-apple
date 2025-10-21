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
                    description: "Promotion coupon discount 10% in summer 2025",
                    productClassification: EProductClassification.IPHONE,
                    discountState: EDiscountState.ACTIVE,
                    discountType: EDiscountType.PERCENTAGE,
                    discountValue: 0.1m,
                    maxDiscountAmount: null,
                    availableQuantity: 10,
                    stock: 10
                )
            };
        }
    }
}
