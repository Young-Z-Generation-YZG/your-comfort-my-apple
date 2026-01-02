

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
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public EDiscountState DiscountState { get; private set; } = EDiscountState.INACTIVE;
    public required EProductClassification ProductClassification { get; init; }
    public required EPromotionType PromotionType { get; init; }
    public required EDiscountType DiscountType { get; init; }
    public double DiscountValue { get; private set; } = 0;
    public double? MaxDiscountAmount { get; private set; }
    public required int AvailableQuantity { get; init; }
    public required int Stock { get; init; }
    public DateTime? ExpiredDate { get; private set; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }
    public string? UpdatedBy { get; private set; }

    public static Coupon Create(CouponId couponId,
                                Code code,
                                string title,
                                string description,
                                EProductClassification productClassification,
                                EPromotionType promotionType,
                                EDiscountState discountState,
                                EDiscountType discountType,
                                double discountValue,
                                int stock = 0,
                                string? userId = null,
                                double? maxDiscountAmount = null,
                                DateTime? expiredDate = null,
                                DateTime? createdAt = null,
                                DateTime? updatedAt = null,
                                bool isDeleted = false,
                                DateTime? deletedAt = null,
                                string? deletedBy = null,
                                string? updatedBy = null)
    {
        return new Coupon(couponId)
        {
            UserId = userId,
            Code = code,
            Title = title,
            Description = description,
            ProductClassification = productClassification,
            PromotionType = promotionType,
            DiscountState = discountState,
            DiscountType = discountType,
            DiscountValue = discountValue,
            MaxDiscountAmount = maxDiscountAmount,
            AvailableQuantity = stock,
            Stock = stock,
            ExpiredDate = expiredDate,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = updatedAt ?? DateTime.UtcNow,
            IsDeleted = isDeleted,
            DeletedAt = deletedAt,
            DeletedBy = deletedBy,
            UpdatedBy = updatedBy
        };
    }

    public bool IsExpired()
    {
        return ExpiredDate.HasValue && ExpiredDate.Value < DateTime.UtcNow;
    }

    public bool IsValid()
    {
        return !IsExpired() 
            && AvailableQuantity > 0 
            && DiscountState == EDiscountState.ACTIVE
            && !IsDeleted;
    }

    public void UseCoupon()
    {
        if (AvailableQuantity <= 0)
        {
            throw new InvalidOperationException("Coupon out of stock");
        }
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
            Stock = Stock,
            ExpiredDate = ExpiredDate
        };
    }
}
