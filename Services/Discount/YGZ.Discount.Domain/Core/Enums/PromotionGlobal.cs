

using Ardalis.SmartEnum;

namespace YGZ.Discount.Domain.Core.Enums;

public class PromotionGlobalType : SmartEnum<PromotionGlobalType>
{
    public PromotionGlobalType(string name, int value) : base(name, value) { }

    public static readonly PromotionGlobalType UNKNOWN = new("UNKNOWN", 0);
    public static readonly PromotionGlobalType CATEGORIES = new("CATEGORIES", 0);
    public static readonly PromotionGlobalType PRODUCTS = new("PRODUCTS", 0);
}