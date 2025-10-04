using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Tenants.ValueObjects;

public class TenantId : ValueObject
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; }

    public string? Value => Id?.ToString();

    public TenantId() { }

    private TenantId(ObjectId id)
    {
        Id = id;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id ?? ObjectId.Empty;
    }

    public static TenantId Create()
    {
        return new TenantId(ObjectId.GenerateNewId());
    }

    public static TenantId Of(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Tenant ID cannot be null or empty", nameof(id));
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId format", nameof(id));
        }

        return new TenantId(objectId);
    }

    public static TenantId Of(ObjectId objectId)
    {
        return new TenantId(objectId);
    }
}
