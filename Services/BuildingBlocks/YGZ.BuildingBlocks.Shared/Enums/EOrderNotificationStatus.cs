using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EOrderNotificationStatus : SmartEnum<EOrderNotificationStatus>
{
    public EOrderNotificationStatus(string name, int value) : base(name, value) { }

    public static readonly EOrderNotificationStatus UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EOrderNotificationStatus PENDING = new(nameof(PENDING), 0);
    public static readonly EOrderNotificationStatus PREPARING = new(nameof(PREPARING), 0);
    public static readonly EOrderNotificationStatus DELIVERING = new(nameof(DELIVERING), 0);
    public static readonly EOrderNotificationStatus DELIVERED = new(nameof(DELIVERED), 0);
    public static readonly EOrderNotificationStatus PAID = new(nameof(PAID), 0);

}
