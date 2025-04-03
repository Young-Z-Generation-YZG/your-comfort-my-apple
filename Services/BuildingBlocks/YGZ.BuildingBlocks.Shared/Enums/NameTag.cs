

using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class ProductNameTag : SmartEnum<ProductNameTag>
{
    public ProductNameTag(string name, int value) : base(name, value) { }

    public static readonly ProductNameTag UNKNOWN = new("UNKNOWN", 0);
    public static readonly ProductNameTag MACBOOK = new("MACBOOK", 1);
    public static readonly ProductNameTag IPAD = new("IPAD", 2);
    public static readonly ProductNameTag IPHONE = new("IPHONE", 3);
    public static readonly ProductNameTag WATCH = new("WATCH", 4);
    public static readonly ProductNameTag HEADPHONE = new("HEADPHONE", 5);
    public static readonly ProductNameTag ACCESSORY = new("ACCESSORY", 6);
}
