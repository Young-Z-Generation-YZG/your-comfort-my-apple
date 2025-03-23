using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class DiscountType : SmartEnum<DiscountType>
{
    public DiscountType(string name, int value) : base(name, value) { }

    // Add a private parameterless constructor for EF Core design time
    private DiscountType() : base("PERCENT", 1) { } // Default to PERCENT or any valid value

    public static readonly DiscountType PERCENT = new("PERCENT", 1);
    public static readonly DiscountType FIXED = new("FIXED", 2);
}