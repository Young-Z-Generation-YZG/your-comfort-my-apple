using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EOrderStatus : SmartEnum<EOrderStatus>
{
    public EOrderStatus(string name, int value) : base(name, value) { }

    public static readonly EOrderStatus PENDING = new(nameof(PENDING), 0);
    public static readonly EOrderStatus CONFIRMED = new(nameof(CONFIRMED), 0);
    public static readonly EOrderStatus PREPARING = new(nameof(PREPARING), 0);
    public static readonly EOrderStatus DELIVERING = new(nameof(DELIVERING), 0);
    public static readonly EOrderStatus CANCELLED = new(nameof(CANCELLED), 0);
    public static readonly EOrderStatus PAID = new(nameof(PAID), 0);
    public static readonly EOrderStatus DELIVERED = new(nameof(DELIVERED), 0);
    public static readonly EOrderStatus PENDING_ASSIGNMENT = new(nameof(PENDING_ASSIGNMENT), 0);
}
