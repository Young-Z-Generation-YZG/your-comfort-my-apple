using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants.Entities;


[BsonCollection("IphoneSkuPrices")]
public class IphoneSkuPrice : Entity<SkuPriceId>, IAuditable, ISoftDelete
{
    public IphoneSkuPrice(SkuPriceId id) : base(id) { }


    [BsonElement("model_id")]
    public required ModelId ModelId { get; init; }

    [BsonElement("unique_query")]
    public string UniqueQuery
    {
        get
        {
            return $"{Model.NormalizedName}_{Storage.NormalizedName}_{Color.NormalizedName}";
        }
    }

    [BsonElement("model")]
    public required Model Model { get; init; }

    [BsonElement("storage")]
    public required Storage Storage { get; init; }

    [BsonElement("color")]
    public required Color Color { get; init; }


    [BsonElement("unit_price")]
    public required decimal UnitPrice { get; init; }

    [BsonElement("cached_key")]
    public string CachedKey
    {
        get
        {
            var cachedKey = CacheKeyPrefixConstants.CatalogService.GetIphoneSkuPriceKey(modelName: EIphoneModel.FromName(Model.NormalizedName),
                                                                                        storageName: EStorage.FromName(Storage.NormalizedName),
                                                                                        colorName: EColor.FromName(Color.NormalizedName));

            return cachedKey;
        }
    }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedBy")]
    public string? UpdatedBy { get; set; } = null;

    [BsonElement("IsDeleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("DeletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [BsonElement("DeletedBy")]
    public string? DeletedBy { get; set; } = null;

    public static IphoneSkuPrice Create(SkuPriceId skuPriceId, ModelId modelId, Model model, Color color, Storage storage, decimal unitPrice)
    {
        return new IphoneSkuPrice(skuPriceId)
        {
            ModelId = modelId,
            Model = model,
            Color = color,
            Storage = storage,
            UnitPrice = unitPrice
        };
    }

    public IphoneSkuPriceResponse ToResponse()
    {
        return new IphoneSkuPriceResponse
        {
            Id = Id.Value!,
            ModelId = ModelId.Value!,
            UniqueQuery = UniqueQuery,
            Model = Model.ToResponse(),
            Color = Color.ToResponse(),
            Storage = Storage.ToResponse(),
            UnitPrice = UnitPrice
        };
    }

}