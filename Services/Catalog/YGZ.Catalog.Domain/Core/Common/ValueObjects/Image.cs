

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Common.ValueObjects;

public class Image : ValueObject
{
    [BsonElement("image_url")]
    public string ImageUrl { get; private set; }

    [BsonElement("image_id")]
    public string ImageId { get; private set; }

    [BsonElement("order")]
    public int Order { get; set; }

    private Image(string url, string id, int order)
    {
        ImageUrl = url;
        ImageId = id;
        Order = order;
    }

    public static Image Create(string url, string id, int order)
    {
        return new Image(url, id, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageUrl;
    }
}
