
using Ardalis.SmartEnum;

namespace YGZ.Ordering.Domain.Core.Enums;

public class PaymentTypeEnum : SmartEnum<PaymentTypeEnum>
{
    public PaymentTypeEnum(string name, int value) : base(name, value) { }

    public static readonly PaymentTypeEnum COD = new(nameof(COD), 0);
    public static readonly PaymentTypeEnum VNPAY = new(nameof(VNPAY), 1);
    public static readonly PaymentTypeEnum MOMO = new(nameof(MOMO), 2);
}
