

using Ardalis.SmartEnum;

namespace YGZ.Basket.Domain.Core.Enums;

public class DiscountTypeEnum : SmartEnum<DiscountTypeEnum>
{
    public DiscountTypeEnum(string name, int value) : base(name, value) { }

    // Add a private parameterless constructor for EF Core design time
    private DiscountTypeEnum() : base("PERCENT", 1) { } // Default to PERCENT or any valid value

    public static readonly DiscountTypeEnum PERCENT = new("PERCENT", 1);
    public static readonly DiscountTypeEnum FIXED = new("FIXED", 2);
}