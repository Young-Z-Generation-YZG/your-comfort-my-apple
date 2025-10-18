using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Domain.Tenants.Entities;

public class IphoneSkuPrice : Entity<SkuPriceId>, IAuditable, ISoftDelete
{
    public IphoneSkuPrice(SkuPriceId id) : base(id) { }

    
    [BsonElement("model_id")]
    public required ModelId ModelId { get; init; }

    [BsonElement("unique_query")]
    public string UniqueQuery { get {
        return $"{Model.NormalizedName}_{Storage.NormalizedName}_{Color.NormalizedName}";
    } }

    [BsonElement("model")]
    public required Model Model { get; init; }

    [BsonElement("color")]
    public required Color Color { get; init; }

    [BsonElement("storage")]
    public required Storage Storage { get; init; }

    [BsonElement("unit_price")]
    public required decimal UnitPrice { get; init; }

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    public string? UpdatedBy { get; init; } = null;

    public bool IsDeleted { get; init; } = false;

    public DateTime? DeletedAt { get; init; } = null;

    public string? DeletedBy { get; init; } = null;

    public static IphoneSkuPrice Create(ModelId modelId, Model model, Color color, Storage storage, decimal unitPrice)
    {
        return new IphoneSkuPrice(SkuPriceId.Create())
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