using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class DiscountType : SmartEnum<DiscountType>
{
    public DiscountType(string name, int value) : base(name, value) { }

    public static readonly DiscountType PERCENTAGE = new("PERCENTAGE", 1);
    public static readonly DiscountType FIXED = new("FIXED", 2);
}