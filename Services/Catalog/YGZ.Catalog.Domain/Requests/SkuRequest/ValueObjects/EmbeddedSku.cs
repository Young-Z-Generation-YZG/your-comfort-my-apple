using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;

public class EmbeddedSku : ValueObject
{
    [BsonElement("sku_id")]
    public required SkuId SkuId { get; init; }

    [BsonElement("model_normalized_name")]
    public required string ModelNormalizedName { get; init; }

    [BsonElement("color_normalized_name")]
    public required string ColorNormalizedName { get; init; }

    [BsonElement("storage_normalized_name")]
    public required string StorageNormalizedName { get; init; }

    [BsonElement("image_url")]
    public required string ImageUrl { get; init; }

    
    public static EmbeddedSku Create(SkuId skuId, Model model, Color color, Storage storage, string imageUrl)
    {
        return new EmbeddedSku
        {
            SkuId = skuId,
            ModelNormalizedName = model.NormalizedName,
            ColorNormalizedName = color.NormalizedName,
            StorageNormalizedName = storage.NormalizedName,
            ImageUrl = imageUrl,
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return SkuId;
        yield return ModelNormalizedName;
        yield return ColorNormalizedName;
        yield return StorageNormalizedName;
        yield return ImageUrl;
    }

    public EmbeddedSkuResponse ToResponse()
    {
        return new EmbeddedSkuResponse
        {
            SkuId = SkuId.Value!,
            ModelNormalizedName = ModelNormalizedName,
            ColorNormalizedName = ColorNormalizedName,
            StorageNormalizedName = StorageNormalizedName,
            ImageUrl = ImageUrl,
        };
    }
}