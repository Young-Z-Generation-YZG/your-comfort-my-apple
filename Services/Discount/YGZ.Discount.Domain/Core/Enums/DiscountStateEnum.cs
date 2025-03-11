using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class DiscountStateEnum : SmartEnum<DiscountStateEnum>
{
    public DiscountStateEnum(string name, int value) : base(name, value) { }

    // Add a private parameterless constructor for EF Core design time
    private DiscountStateEnum() : base("INACTIVE", 1) { } // Default to INACTIVE or any valid value

    public static readonly DiscountStateEnum ACTIVE = new("ACTIVE", 1);
    public static readonly DiscountStateEnum INACTIVE = new("INACTIVE", 2);
    public static readonly DiscountStateEnum EXPIRED = new("EXPIRED", 3);
}