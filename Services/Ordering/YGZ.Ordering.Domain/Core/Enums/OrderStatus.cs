
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public class OrderStatus : SmartEnum<OrderStatus>
{
    public OrderStatus(string name, int value) : base(name, value) { }

    public static readonly OrderStatus PENDING = new(nameof(PENDING), 1);
    public static readonly OrderStatus CONFIRMED = new(nameof(CONFIRMED), 2);
    public static readonly OrderStatus PREPARING = new(nameof(PREPARING), 3);
    public static readonly OrderStatus DELIVERING = new(nameof(DELIVERING), 4);
    public static readonly OrderStatus CANCELLED = new(nameof(CANCELLED), 5);
    public static readonly OrderStatus PAID = new(nameof(PAID), 6);
    public static readonly OrderStatus DELIVERED = new(nameof(DELIVERED), 7);
}
