using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class PromotionCoupon
{
    public required string CouponId { get; init; }
        public required decimal ProductUnitPrice { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
     public decimal FinalPrice {
        get {
            if (DiscountType == EDiscountType.PERCENTAGE.Name)
            {
                return ProductUnitPrice - (ProductUnitPrice * DiscountValue);
            }
            else
            {
                return ProductUnitPrice - DiscountValue;
            }
        }
    }

    public static PromotionCoupon Create(string couponId, decimal productUnitPrice, EDiscountType discountType, decimal discountValue, decimal discountAmount)
    {
        return new PromotionCoupon
        {
            CouponId = couponId,
            ProductUnitPrice = productUnitPrice,
            DiscountType = discountType.Name,
            DiscountValue = discountValue,
            DiscountAmount = discountAmount,
        };
    }

        public PromotionResponse ToResponse()
    {
        return new PromotionResponse
        {
            PromotionType = EPromotionType.COUPON.Name,
            ProductUnitPrice = ProductUnitPrice,
            DiscountType = DiscountType,
            DiscountValue = DiscountValue,
            DiscountAmount = DiscountAmount,
            FinalPrice = FinalPrice,
        };
    }
}
