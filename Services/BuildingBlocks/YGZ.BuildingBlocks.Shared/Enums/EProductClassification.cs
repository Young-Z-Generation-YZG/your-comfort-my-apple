using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EProductClassification : SmartEnum<EProductClassification>
{
    public EProductClassification(string name, int value) : base(name, value) { }

    public static readonly EProductClassification UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EProductClassification IPHONE = new(nameof(IPHONE), 0);
    public static readonly EProductClassification IPAD = new(nameof(IPAD), 0);
    public static readonly EProductClassification MACBOOK = new(nameof(MACBOOK), 0);
    public static readonly EProductClassification WATCH = new(nameof(WATCH), 0);
    public static readonly EProductClassification HEADPHONE = new(nameof(HEADPHONE), 0);
    public static readonly EProductClassification ACCESSORY = new(nameof(ACCESSORY), 0);
}