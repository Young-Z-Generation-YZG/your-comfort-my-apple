using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class ESkuState : SmartEnum<ESkuState>
{
    public ESkuState(string name, int value) : base(name, value) { }

    public static readonly ESkuState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly ESkuState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly ESkuState INACTIVE = new(nameof(INACTIVE), 0);
}