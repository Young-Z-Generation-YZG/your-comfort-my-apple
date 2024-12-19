using GYZ.Discount.Grpc.Common.Enums;

namespace GYZ.Discount.Grpc.Entities;

public class Coupon
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DiscountTypeEnum Type { get; set; } = DiscountTypeEnum.PERCENT;
    public DiscountStatusEnum Status { get; set; } = DiscountStatusEnum.INACTIVE;
    public double DiscountValue { get; set; }
    public double? MinPurchaseAmount { get; set; }
    public double? MaxDiscountAmount { get; set; }
    public DateTime? ValidFrom { get; set; } = null;
    public DateTime? ValidTo { get; set; } = null;
    public int QuantityRemain { get; set; }
    public int UsageLimit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; } = null;

    public Coupon() { }


    public Coupon(Guid id,
                   string code,
                   string title,
                   string description,
                   double discountValue,
                   double? minPurchaseAmount,
                   double? maxDiscountAmount,
                   DateTime? validFrom,
                   DateTime? validTo,
                   int usageLimit,
                   DateTime createdAt,
                   DateTime updatedAt)
    {
        Id = id;
        Code = code;
        Title = title;
        Description = description;
        DiscountValue = discountValue;
        MinPurchaseAmount = minPurchaseAmount;
        MaxDiscountAmount = maxDiscountAmount;
        ValidFrom = validFrom;
        ValidTo = validTo;
        QuantityRemain = usageLimit;
        UsageLimit = usageLimit;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Coupon CreateNew(
        string code,
        string title,
        string description,
        double discountValue,
        double? minPurchaseAmount,
        double? maxDiscountAmount,
        DateTime? validFrom,
        DateTime? validTo,
        int usageLimit)
    {
        return new Coupon(
            Guid.NewGuid(),
            code,
            title,
            description,
            discountValue,
            minPurchaseAmount > 0 ? minPurchaseAmount : null,
            maxDiscountAmount > 0 ? maxDiscountAmount : null,
            validFrom,
            validTo,
            usageLimit,
            DateTime.UtcNow.ToUniversalTime(),
            DateTime.UtcNow.ToUniversalTime()
            );
    }

    public static Coupon ToUpdate(Guid id,
                                string title,
                                string description,
                                double discountValue,
                                double? minPurchaseAmount,
                                double? maxDiscountAmount,
                                DateTime? validFrom,
                                DateTime? validTo,
                                int usageLimit)
    {
        return new Coupon(
            id,
            string.Empty,
            title,
            description,
            discountValue,
            minPurchaseAmount > 0 ? minPurchaseAmount : null,
            maxDiscountAmount > 0 ? maxDiscountAmount : null,
            validFrom,
            validTo,
            usageLimit,
            DateTime.UtcNow.ToUniversalTime(),
            DateTime.UtcNow.ToUniversalTime()
            );
    }
} 
