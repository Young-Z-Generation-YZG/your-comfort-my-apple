using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class EQuantityState : SmartEnum<EQuantityState>
{
    public EQuantityState(string name, int value) : base(name, value) { }

    public static readonly EQuantityState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EQuantityState OUT_OF_STOCK = new(nameof(OUT_OF_STOCK), 0);
    public static readonly EQuantityState IPAD = new(nameof(IPAD), 0);
}