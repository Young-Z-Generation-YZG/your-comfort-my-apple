using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EPaymentMethod : SmartEnum<EPaymentMethod>
{
    public EPaymentMethod(string name, int value) : base(name, value) { }

    public static readonly EPaymentMethod UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EPaymentMethod COD = new(nameof(COD), 0);
    public static readonly EPaymentMethod VNPAY = new(nameof(VNPAY), 0);
    public static readonly EPaymentMethod MOMO = new(nameof(MOMO), 0);
    public static readonly EPaymentMethod BLOCKCHAIN = new(nameof(BLOCKCHAIN), 0);
}