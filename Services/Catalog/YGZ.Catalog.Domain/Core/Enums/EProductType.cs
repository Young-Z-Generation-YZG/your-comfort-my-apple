
using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class EProductType : SmartEnum<EProductType>
{
    public EProductType(string name, int value) : base(name, value) { }

    public static readonly EProductType UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EProductType IPHONE = new(nameof(IPHONE), 0);
    public static readonly EProductType IPAD = new(nameof(IPAD), 0);
    public static readonly EProductType MACBOOK = new(nameof(MACBOOK), 0);
    public static readonly EProductType WATCH = new(nameof(WATCH), 0);
    public static readonly EProductType HEADPHONE = new(nameof(HEADPHONE), 0);
    public static readonly EProductType ACCESSORY = new(nameof(ACCESSORY), 0);
}
