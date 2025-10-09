using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EPromotionType : SmartEnum<EPromotionType>
{
    public EPromotionType(string name, int value) : base(name, value) { }

    public static readonly EPromotionType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EPromotionType COUPON = new(nameof(COUPON), 0);
    public static readonly EPromotionType EVENT = new(nameof(EVENT), 0);
}
