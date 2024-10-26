
using YGZ.Catalog.Domain.Core.Base;

namespace YGZ.Catalog.Domain.Products.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string[] Image_urls { get; set; } = [];
    public string[] Image_ids { get; set; } = [];
    public Usb Usb { get; set; } = new Usb();
    public string Security_feature { get; set; } = "";
    public string Slug { get; set; } = "";
}

public class Usb
{
    public string Type { get; set; } = USBTypes.USBC;
    public string Version { get; set; } = "";
}

public interface USBTypes
{
    public const string USBC = "USB-C";
    public const string Lightning = "Lightning";
}

public static class USBVersions
{
    public const string USB2 = "USB 2";
    public const string USB3 = "USB 3";
}

public static class SecurityFeatures
{
    public const string FaceID = "Face ID";
    public const string TouchID = "Touch ID";
}

