using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class DiscountTypeEnum : SmartEnum<DiscountTypeEnum>
{
    public DiscountTypeEnum(string name, int value) : base(name, value) { }

    public static readonly DiscountTypeEnum PERCENT = new("PERCENT", 0);
    public static readonly DiscountTypeEnum FIXED = new("FIXED", 1);
}