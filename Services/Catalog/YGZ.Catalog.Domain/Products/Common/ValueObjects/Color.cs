
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

    [BsonElement("order")]
    public int Order { get; init; } = 0;

    private Color(string name, string hexCode, int order)
    {
        Name = name;
        HexCode = hexCode;
        Order = order;
    }

    public static Color Create(string name, string hexCode, int order)
    {
        EColor.TryFromName(name, out var color);

        if (color is null)
        {
            throw new ArgumentException("Invalid EColor ${name}", name);
        }

        return new Color(name, hexCode, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return HexCode;
        yield return Order;
    }
}
