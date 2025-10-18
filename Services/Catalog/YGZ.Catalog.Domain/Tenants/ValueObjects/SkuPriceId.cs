using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Tenants.ValueObjects;

public class SkuPriceId : ValueObject
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; }

    public string? Value => Id?.ToString();

    public SkuPriceId() { }

    private SkuPriceId(ObjectId id)
    {
        Id = id;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id ?? ObjectId.Empty;
    }

    public static SkuPriceId Create()
    {
        return new SkuPriceId(ObjectId.GenerateNewId());
    }

    public static SkuPriceId Of(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Sku Price ID cannot be null or empty", nameof(id));
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid Sku Price ID format", nameof(id));
        }

        return new SkuPriceId(objectId);
    }

    public static SkuPriceId Of(ObjectId objectId)
    {
        return new SkuPriceId(objectId);
    }
}