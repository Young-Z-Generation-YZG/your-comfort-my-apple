﻿

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Color : ValueObject
{
    [BsonElement("color_name")]
    public string ColorName { get; set; }

    [BsonElement("color_hex")]
    public string ColorHex { get; set; }

    [BsonElement("color_Image")]
    public string ColorImage { get; set; }

    [BsonElement("color_order")]
    public int? ColorOrder { get; set; }

    private Color(string name, string hex, string image, int? order)
    {
        ColorName = name;
        ColorHex = hex;
        ColorImage = image;
        ColorOrder = order;
    }

    public static Color Create(string colorName, string colorHex, string colorImage, int? colorOrder)
    {
        return new Color(colorName, colorHex, colorImage, colorOrder);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ColorName;
        yield return ColorHex;
        yield return ColorImage;
        yield return ColorOrder;
    }
}
