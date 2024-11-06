

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Common.ValueObjects;

public class Image : ValueObject
{
    [BsonElement("image_url")]
    public string ImageUrl { get; }

    [BsonElement("image_id")]
    public string ImageId { get; }

    private Image(string url, string id)
    {
        ImageUrl = url;
        ImageId = id;
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
