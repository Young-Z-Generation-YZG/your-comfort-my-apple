
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public static partial class Enums
{
    public class OrderStatus : SmartEnum<OrderStatus>
    {
        public OrderStatus(string name, int value) : base(name, value) { }

        public static readonly OrderStatus UN_PAID = new(nameof(UN_PAID), 0);
    }
}
