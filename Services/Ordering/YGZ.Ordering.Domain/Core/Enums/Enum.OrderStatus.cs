
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public static partial class Enums
{
    public class OrderStatus : SmartEnum<OrderStatus>
    {
        public OrderStatus(string name, int value) : base(name, value) { }

        public static readonly OrderStatus PENDING = new(nameof(PENDING), 0);
        public static readonly OrderStatus CONFIRMED = new(nameof(CONFIRMED), 1);
        public static readonly OrderStatus SHIPPING = new(nameof(SHIPPING), 2);
        public static readonly OrderStatus DELIVERED = new(nameof(DELIVERED), 3);
        public static readonly OrderStatus CANCELLED = new(nameof(CANCELLED), 4);
        public static readonly OrderStatus RETURNED = new(nameof(RETURNED), 5);

        public static readonly OrderStatus WAITTING_FOR_PAY = new(nameof(WAITTING_FOR_PAY), 7);
        public static readonly OrderStatus PAID = new(nameof(PAID), 6);
    }
}
