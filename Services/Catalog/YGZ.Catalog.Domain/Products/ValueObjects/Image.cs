

using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.ValueObjects;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class Image : ValueObject
{
    [BsonElement("image_id")]
    public string ImageId { get; private set; }

    [BsonElement("image_url")]
    public string ImageUrl { get; private set; }


    [BsonElement("image_order")]
    public int ImageOrder { get; private set; }

    private Image(string id, string url, int order)
    {
        ImageId = id;
        ImageUrl = url;
        ImageOrder = order;
    }

    public static Image Create(string imageId, string imageUrl, int imageOrder)
    {
        return new Image(imageId, imageUrl, imageOrder);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageId;
        yield return ImageUrl;
        yield return ImageOrder;

    }
}
