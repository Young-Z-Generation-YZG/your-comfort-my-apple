

using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class DiscountStateEnum : SmartEnum<DiscountStateEnum>
{
    public DiscountStateEnum(string name, int value) : base(name, value) { }

    public static readonly DiscountStateEnum ACTIVE = new("ACTIVE", 0);
    public static readonly DiscountStateEnum INACTIVE = new("INACTIVE", 1);
    public static readonly DiscountStateEnum EXPIRED = new("EXPIRED", 2);
}