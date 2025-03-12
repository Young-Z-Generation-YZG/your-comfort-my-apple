
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public class OrderStatusEnum : SmartEnum<OrderStatusEnum>
{
    public OrderStatusEnum(string name, int value) : base(name, value) { }

    public static readonly OrderStatusEnum PENDING = new(nameof(PENDING), 0);
    public static readonly OrderStatusEnum PAID = new(nameof(PAID), 1);
}
