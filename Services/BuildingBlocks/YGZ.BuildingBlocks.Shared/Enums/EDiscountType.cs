using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EDiscountType : SmartEnum<EDiscountType>
{
    public EDiscountType(string name, int value) : base(name, value) { }

    public static readonly EDiscountType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EDiscountType PERCENTAGE = new(nameof(PERCENTAGE), 1);
    public static readonly EDiscountType FIXED = new(nameof(FIXED), 2);
}
