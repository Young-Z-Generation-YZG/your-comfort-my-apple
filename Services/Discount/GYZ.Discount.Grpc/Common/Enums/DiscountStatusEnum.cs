using Ardalis.SmartEnum;

namespace GYZ.Discount.Grpc.Common.Enums;

public class DiscountStatusEnum : SmartEnum<DiscountStatusEnum>
{
    public DiscountStatusEnum(string name, int value) : base(name, value) { }

    public static readonly DiscountStatusEnum ACTIVE = new("ACTIVE", 0);
    public static readonly DiscountStatusEnum INACTIVE = new("INACTIVE", 1);
    public static readonly DiscountStatusEnum EXPIRED = new("EXPIRED", 2);
}