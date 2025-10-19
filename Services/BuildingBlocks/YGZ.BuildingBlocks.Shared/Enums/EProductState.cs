using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EProductState : SmartEnum<EProductState>
{
    public EProductState(string name, int value) : base(name, value) { }

    public static readonly EProductState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EProductState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly EProductState INACTIVE = new(nameof(INACTIVE), 0);
    public static readonly EProductState DRAFT = new(nameof(DRAFT), 0);
    public static readonly EProductState DELETED = new(nameof(DELETED), 0);
    public static readonly EProductState OUT_OF_STOCK = new(nameof(OUT_OF_STOCK), 0);
    public static readonly EProductState IN_STOCK = new(nameof(IN_STOCK), 0);
}
