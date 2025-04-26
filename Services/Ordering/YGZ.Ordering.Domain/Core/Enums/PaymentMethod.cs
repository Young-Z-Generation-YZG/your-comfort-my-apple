
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public class PaymentMethod : SmartEnum<PaymentMethod>
{
    public PaymentMethod(string name, int value) : base(name, value) { }

    public static readonly PaymentMethod COD = new(nameof(COD), 0);
    public static readonly PaymentMethod VNPAY = new(nameof(VNPAY), 1);
    public static readonly PaymentMethod MOMO = new(nameof(MOMO), 2);
}
