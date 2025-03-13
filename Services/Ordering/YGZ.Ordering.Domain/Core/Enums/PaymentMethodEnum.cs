
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public class PaymentMethodEnum : SmartEnum<PaymentMethodEnum>
{
    public PaymentMethodEnum(string name, int value) : base(name, value) { }

    public static readonly PaymentMethodEnum COD = new(nameof(COD), 0);
    public static readonly PaymentMethodEnum VNPAY = new(nameof(VNPAY), 1);
    public static readonly PaymentMethodEnum MOMO = new(nameof(MOMO), 2);
}
