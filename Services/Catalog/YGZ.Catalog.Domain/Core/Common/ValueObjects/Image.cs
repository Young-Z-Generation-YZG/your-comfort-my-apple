

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Common.ValueObjects;

public class Image : ValueObject
{
    [BsonElement("url")]
    public string Url { get; }
    [BsonElement("id")]
    public string Id { get; }

    private Image(string url, string id)
    {
        Url = url;
        Id = id;
    }

    public static Image Create(string url, string id)
    {
        return new Image(url, id);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
