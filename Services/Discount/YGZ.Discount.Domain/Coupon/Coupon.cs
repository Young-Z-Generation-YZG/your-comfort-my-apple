

using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Domain.Coupons;

public class Coupon : AggregateRoot<CouponId>, IAuditable, ISoftDelete
{
    public Coupon(CouponId id) : base(id) { }

    public string? UserId { get; init; }
    public required Code Code { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required EDiscountState DiscountState { get; init; } = EDiscountState.ACTIVE;
    public required EProductClassification ProductClassification { get; init; }
    public EPromotionType PromotionType { get; init; } = EPromotionType.COUPON;
    public required EDiscountType DiscountType { get; init; } = EDiscountType.PERCENTAGE;
    public required decimal DiscountValue { get; init; }
    public required decimal? MaxDiscountAmount { get; init; }
    public int AvailableQuantity { get; private set; }
    public required int Stock { get; init; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public string? UpdatedBy => throw new NotImplementedException();

    public static Coupon Create(CouponId couponId,
                                string? userId,
                                Code code,
                                string title,
                                string description,
                                EProductClassification productClassification,
                                EDiscountState discountState,
                                EDiscountType discountType,
                                decimal discountValue,
                                decimal? maxDiscountAmount,
                                int availableQuantity,
                                int stock)
    {
        return new Coupon(couponId)
        {
            UserId = userId,
            Code = code,
            Title = title,
            Description = description,
            ProductClassification = productClassification,
            DiscountState = discountState,
            DiscountType = discountType,
            DiscountValue = discountValue,
            MaxDiscountAmount = maxDiscountAmount,
            AvailableQuantity = availableQuantity,
            Stock = stock
        };
    }

    public void UseCoupon()
    {
        if (AvailableQuantity <= 0)
        {
            throw new InvalidOperationException("Coupon out of stock");
        }

        AvailableQuantity--;
    }

    public CouponResponse ToResponse()
    {
        return new CouponResponse
        {
            Id = Id.Value.ToString()!,
            UserId = UserId,
            Title = Title,
            Code = Code.Value,
            Description = Description,
            ProductClassification = ProductClassification.Name,
            PromotionType = PromotionType.Name,
            DiscountState = DiscountState.Name,
            DiscountType = DiscountType.Name,
            DiscountValue = DiscountValue,
            MaxDiscountAmount = MaxDiscountAmount,
            AvailableQuantity = AvailableQuantity,
            Stock = Stock
        };
    }
}
