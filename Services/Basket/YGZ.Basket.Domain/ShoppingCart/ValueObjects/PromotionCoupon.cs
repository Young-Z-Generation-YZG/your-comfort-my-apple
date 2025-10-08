namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class PromotionCoupon
{
    public required string CouponId { get; init; }

    public static PromotionCoupon Create(string couponId)
    {
        return new PromotionCoupon
        {
            CouponId = couponId,
        };
    }
}
