
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class Color : ValueObject
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("color_hash")]
    public string ColorHash { get; set; }

    [BsonElement("order")]
    public int Order { get; set; }

    private Color(string name, string colorHash, int order)
    {
        Name = name;
        ColorHash = colorHash;
        Order = order;
    }

    public static Color CreateNew(string name, string colorHash, int order)
    {
        return new Color(name, colorHash, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return ColorHash;
        yield return Order;
    }
}
