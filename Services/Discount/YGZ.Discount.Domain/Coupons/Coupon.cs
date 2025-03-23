

using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Domain.Coupons;

public class Coupon : Entity<Code>, IAuditable, ISoftDelete
{
    public Coupon(Code code) : base(code)
    {

    }

    // Parameterless constructor for EF Core design time
    private Coupon() : base(null!) // Pass null temporarily, will be set via properties
    {

    }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DiscountState State { get; set; } = DiscountState.INACTIVE;
    public NameTag ProductNameTag { get; set; }
    public DiscountType Type { get; set; } = DiscountType.PERCENT;
    public double DiscountValue { get; set; } = 0;
    public double? MaxDiscountAmount { get; set; } = null;
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int AvailableQuantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedByUserId { get; set; } = null;

    public static Coupon Create(string code,
                                string title,
                                string description,
                                DiscountType type,
                                double discountValue,
                                NameTag nameTag,
                                double? maxDiscountAmount,
                                DateTime? validFrom,
                                DateTime? validTo,
                                int availableQuantity)
    {
        return new Coupon(Code.Create(code))
        {
            Title = title,
            Description = description,
            Type = type,
            DiscountValue = discountValue,
            ProductNameTag = nameTag,
            MaxDiscountAmount = maxDiscountAmount,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity
        };
    }

    public static Coupon Update(string code,
                                string title,
                               string description,
                               DiscountState state,
                               NameTag nameTag,
                               DiscountType type,
                               double discountValue,
                               double? maxDiscountAmount,
                               DateTime? validFrom,
                               DateTime? validTo,
                               int availableQuantity)
    {
        return new Coupon(Code.Create(code))
        {
            Title = title,
            Description = description,
            Type = type,
            State = state,
            DiscountValue = discountValue,
            ProductNameTag = nameTag,
            MaxDiscountAmount = maxDiscountAmount,
            ValidFrom = validFrom,
            ValidTo = validTo,
            AvailableQuantity = availableQuantity
        };
    }
}
