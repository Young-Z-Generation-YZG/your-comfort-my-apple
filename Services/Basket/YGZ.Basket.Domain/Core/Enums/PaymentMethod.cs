

using Ardalis.SmartEnum;

namespace YGZ.Basket.Domain.Core.Enums;

public class PaymentMethod : SmartEnum<DiscountType>
{
    public PaymentMethod(string name, int value) : base(name, value) { }

    public static readonly PaymentMethod UNKNOWN = new("UNKNOWN", 0);
    public static readonly PaymentMethod COD = new("COD", 1);
    public static readonly PaymentMethod VNPAY = new("VNPAY", 2);
    public static readonly PaymentMethod MOMO = new("MOMO", 3);
}