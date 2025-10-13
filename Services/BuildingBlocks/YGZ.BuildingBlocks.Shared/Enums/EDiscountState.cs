using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EDiscountState : SmartEnum<EDiscountState>
{
    public EDiscountState(string name, int value) : base(name, value) { }

    public static readonly EDiscountState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EDiscountState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly EDiscountState INACTIVE = new(nameof(INACTIVE), 0);
    public static readonly EDiscountState EXPIRED = new(nameof(EXPIRED), 0);
    public static readonly EDiscountState INSUFFICIENT_STOCK = new(nameof(INSUFFICIENT_STOCK), 0);
}