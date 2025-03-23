

using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class NameTag : SmartEnum<NameTag>
{
    public NameTag(string name, int value) : base(name, value) { }

    public static readonly NameTag MACBOOK = new("MACBOOK", 1);
    public static readonly NameTag IPAD = new("IPAD", 2);
    public static readonly NameTag IPHONE = new("IPHONE", 3);
    public static readonly NameTag WATCH = new("WATCH", 4);
    public static readonly NameTag HEADPHONE = new("HEADPHONE", 5);
    public static readonly NameTag ACCESSORY = new("ACCESSORY", 6);
}
