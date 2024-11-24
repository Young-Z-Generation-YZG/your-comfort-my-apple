
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class Color : ValueObject
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("color_hash")]
    public string ColorHash { get; set; }

    private Color(string name, string colorHash)
    {
        Name = name;
        ColorHash = colorHash;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return ColorHash;
    }
}
