using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class ECategoryType : SmartEnum<ECategoryType>
{
    public ECategoryType(string name, int value) : base(name, value) { }

    public static readonly ECategoryType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly ECategoryType IPHONE = new(nameof(IPHONE), 0);
    public static readonly ECategoryType IPAD = new(nameof(IPAD), 0);
    public static readonly ECategoryType MACBOOK = new(nameof(MACBOOK), 0);
    public static readonly ECategoryType WATCH = new(nameof(WATCH), 0);
    public static readonly ECategoryType HEADPHONE = new(nameof(HEADPHONE), 0);
    public static readonly ECategoryType ACCESSORY = new(nameof(ACCESSORY), 0);
}