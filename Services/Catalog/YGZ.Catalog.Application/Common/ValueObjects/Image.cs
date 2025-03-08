

using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.ValueObjects;

namespace YGZ.Catalog.Application.Common.ValueObjects;

public class Image : ValueObject
{
    [BsonElement("image_id")]
    public string ImageId { get; private set; }

    [BsonElement("image_url")]
    public string ImageUrl { get; private set; }

    [BsonElement("order")]
    public int Order { get; set; }

    private Image(string url, string id, int order)
    {
        ImageId = id;
        ImageUrl = url;
        Order = order;
    }

    public static Image Create(string id, string url, int order)
    {
        return new Image(id, url, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageUrl;
    }
}
