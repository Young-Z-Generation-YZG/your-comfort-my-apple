

using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Domain.Coupons;

public class Coupon : Entity<CouponId>, IAuditable, ISoftDelete
{
    public Coupon(CouponId id) : base(id)
    {

    }

    public string Title { get; set; }
    public string Description { get; set; }
    public DiscountTypeEnum Type { get; set; } = DiscountTypeEnum.PERCENT;
    public DiscountStateEnum Status { get; set; } = DiscountStateEnum.INACTIVE;
    public double DiscountValue { get; set; }
    public double? MinPurchaseAmount { get; set; } = null;
    public double? MaxDiscountAmount { get; set; } = null;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int AvailableQuantity { get; set; }

    public DateTime CreatedAt => CouponId.CreatedTime;

    public DateTime UpdatedAt => CouponId.CreatedTime;

    public bool IsDeleted => false;

    public DateTime? DeletedAt => null;

    public string? DeletedByUserId => null;
}
