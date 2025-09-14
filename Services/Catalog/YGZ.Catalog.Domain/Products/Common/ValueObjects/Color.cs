
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Color : ValueObject
{
    [BsonElement("name")]
    public string Name { get; init; }

    [BsonElement("hex_code")]
    public string HexCode { get; init; }

    [BsonElement("showcase_image_id")]
    public string ShowcaseImageId { get; init; }

    [BsonElement("order")]
    public int Order { get; init; } = 0;

    private Color(string name, string hexCode, string showcaseImageId, int order)
    {
        Name = name;
        HexCode = hexCode;
        ShowcaseImageId = showcaseImageId;
        Order = order;
    }

    public static Color Create(string name, string hexCode, string showcaseImageId, int order)
    {
        EColor.TryFromName(name, out var color);

        if (color is null)
        {
            throw new ArgumentException("Invalid color name", name);
        }

        return new Color(name, hexCode, showcaseImageId, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return HexCode;
        yield return ShowcaseImageId;
        yield return Order;
    }
}
