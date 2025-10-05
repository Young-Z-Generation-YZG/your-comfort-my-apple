

using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Image : ValueObject
{
    [BsonElement("image_id")]
    public string ImageId { get; private set; }

    [BsonElement("image_url")]
    public string ImageUrl { get; private set; }

    [BsonElement("image_name")]
    public string ImageName { get; private set; }

    [BsonElement("image_description")]
    public string ImageDescription { get; private set; }

    [BsonElement("image_w")]
    public decimal ImageWidth { get; private set; }

    [BsonElement("image_h")]
    public decimal ImageHeight { get; private set; }

    [BsonElement("image_bytes")]
    public decimal ImageBytes { get; private set; }

    [BsonElement("image_order")]
    public int? ImageOrder { get; private set; }

    private Image(string id, string url, string name, string description, decimal width, decimal height, decimal bytes, int? order)
    {
        ImageId = id;
        ImageUrl = url;
        ImageName = name;
        ImageDescription = description;
        ImageWidth = width;
        ImageHeight = height;
        ImageBytes = bytes;
        ImageOrder = order;
    }

    public static Image Create(string imageId,
                               string imageUrl,
                               string imageName,
                               string imageDescription,
                               decimal imageWidth,
                               decimal imageHeight,
                               decimal imageBytes,
                               int? imageOrder)
    {
        return new Image(imageId, imageUrl, imageName, imageDescription, imageWidth, imageHeight, imageBytes, imageOrder);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ImageId;
        yield return ImageUrl;
        yield return ImageName;
        yield return ImageWidth;
        yield return ImageHeight;
        yield return ImageBytes;
        yield return ImageOrder;
    }

    public ImageResponse ToResponse()
    {
        return new ImageResponse
        {
            ImageId = ImageId,
            ImageUrl = ImageUrl,
            ImageName = ImageName,
            ImageDescription = ImageDescription,
            ImageWidth = ImageWidth,
            ImageHeight = ImageHeight,
            ImageBytes = ImageBytes,
            ImageOrder = ImageOrder
        };
    }
}
