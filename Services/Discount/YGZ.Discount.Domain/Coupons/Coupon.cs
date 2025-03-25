

using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Domain.Coupons;

public class Coupon : AggregateRoot<CouponId>, IAuditable, ISoftDelete
{
    public Coupon(CouponId id) : base(id)
    {

    }

    // Parameterless constructor for EF Core design time
    private Coupon() : base(null!) { }

    public Code Code { get; set; } = default!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DiscountState DiscountState { get; set; } = DiscountState.INACTIVE;
    public ProductNameTag ProductNameTag { get; set; }
    public PromotionEventType PromotionEventType { get; set; } = PromotionEventType.PROMOTION_COUPON;
    public DiscountType DiscountType { get; set; } = DiscountType.PERCENTAGE;
    public decimal DiscountValue { get; set; } = 0;
    public decimal? MaxDiscountAmount { get; set; } = null;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int AvailableQuantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static Coupon Create(CouponId couponId,
                                Code code,
                                string title,
                                string description,
                                ProductNameTag nameTag,
                                PromotionEventType promotionEventType,
                                DiscountState discountState,
                                DiscountType discountType,
                                decimal discountValue,
                                decimal? maxDiscountAmount,
                                DateTime? validFrom,
                                DateTime? validTo,
                                int availableQuantity)
    {
        return new Coupon(couponId)
        {
            Code = code,
            Title = title,
            Description = description,
            ProductNameTag = nameTag,
            PromotionEventType = promotionEventType,
            DiscountState = discountState,
            DiscountType = discountType,
            DiscountValue = discountValue,
            MaxDiscountAmount = maxDiscountAmount,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity
        };
    }

    public static Coupon Update(CouponId couponId,
                                Code code,
                                string title,
                                string description,
                                DiscountState discountState,
                                ProductNameTag productNameTag,
                                DiscountType discountType,
                                decimal discountValue,
                                decimal? maxDiscountAmount,
                                DateTime? validFrom,
                                DateTime? validTo,
                                int availableQuantity)
    {
        return new Coupon(couponId)
        {
            Code = code,
            Title = title,
            Description = description,
            DiscountType = discountType,
            DiscountState = discountState,
            DiscountValue = discountValue,
            ProductNameTag = productNameTag,
            MaxDiscountAmount = maxDiscountAmount,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity
        };
    }
}
