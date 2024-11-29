

using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public static partial class Enums
{
    public class PaymentType : SmartEnum<PaymentType>
    {
        public PaymentType(string name, int value) : base(name, value) { }

        public static readonly PaymentType COD = new(nameof(COD), 0);
        public static readonly PaymentType VNPAY = new(nameof(VNPAY), 1);
        public static readonly PaymentType MOMO = new(nameof(MOMO), 2);
    }
}
