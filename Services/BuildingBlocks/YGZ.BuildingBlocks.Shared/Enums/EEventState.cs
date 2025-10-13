using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EEventState : SmartEnum<EEventState>
{
    public EEventState(string name, int value) : base(name, value) { }

    public static readonly EEventState UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EEventState ACTIVE = new(nameof(ACTIVE), 0);
    public static readonly EEventState INACTIVE = new(nameof(INACTIVE), 0);
}