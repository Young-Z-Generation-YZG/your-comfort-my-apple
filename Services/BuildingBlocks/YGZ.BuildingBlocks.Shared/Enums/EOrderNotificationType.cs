using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EOrderNotificationType : SmartEnum<EOrderNotificationType>
{
    public EOrderNotificationType(string name, int value) : base(name, value) { }

    public static readonly EOrderNotificationType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EOrderNotificationType ORDER_STATUS_UPDATED = new(nameof(ORDER_STATUS_UPDATED), 0);
    public static readonly EOrderNotificationType ORDER_CREATED = new(nameof(ORDER_CREATED), 0);
}
