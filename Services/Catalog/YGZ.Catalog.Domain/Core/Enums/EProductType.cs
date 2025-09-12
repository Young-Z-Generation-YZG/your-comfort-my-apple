
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EProductType : SmartEnum<EProductType>
{
    public EProductType(string name, int value) : base(name, value) { }

    public static readonly EProductType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EProductType IPHONE = new(nameof(IPHONE), 1);
    public static readonly EProductType IPAD = new(nameof(IPAD), 2);
    public static readonly EProductType MACBOOK = new(nameof(MACBOOK), 3);
    public static readonly EProductType WATCH = new(nameof(WATCH), 4);
    public static readonly EProductType HEADPHONE = new(nameof(HEADPHONE), 5);
    public static readonly EProductType ACCESSORY = new(nameof(ACCESSORY), 6);
}
