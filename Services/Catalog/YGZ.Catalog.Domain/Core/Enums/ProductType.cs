

using Ardalis.SmartEnum;

namespace YGZ.Catalog.Domain.Core.Enums;

public class ProductType : SmartEnum<ProductType>
{
    public ProductType(string name, int value) : base(name, value) { }

    public static readonly ProductType UNKNOWN = new("UNKNOWN", 0);
    public static readonly ProductType MACBOOK = new("MACBOOK", 1);
    public static readonly ProductType IPAD = new("IPAD", 2);
    public static readonly ProductType IPHONE = new("IPHONE", 3);
    public static readonly ProductType WATCH = new("WATCH", 4);
    public static readonly ProductType HEADPHONE = new("HEADPHONE", 5);
    public static readonly ProductType ACCESSORY = new("ACCESSORY", 6);
}
