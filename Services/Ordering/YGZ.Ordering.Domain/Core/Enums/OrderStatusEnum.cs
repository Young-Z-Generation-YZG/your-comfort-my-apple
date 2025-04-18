﻿
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public class OrderStatusEnum : SmartEnum<OrderStatusEnum>
{
    public OrderStatusEnum(string name, int value) : base(name, value) { }

    public static readonly OrderStatusEnum PENDING = new(nameof(PENDING), 0);
    public static readonly OrderStatusEnum CANCELLED = new(nameof(CANCELLED), 1);
    public static readonly OrderStatusEnum PAID = new(nameof(PAID), 2);
    public static readonly OrderStatusEnum DELIVERED = new(nameof(DELIVERED), 2);
}
