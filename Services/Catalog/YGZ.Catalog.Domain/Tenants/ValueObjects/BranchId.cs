using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Tenants.ValueObjects;

public class BranchId : ValueObject
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; }

    public string? Value => Id?.ToString();

    public BranchId() { }

    private BranchId(ObjectId id)
    {
        Id = id;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id ?? ObjectId.Empty;
    }

    public static BranchId Create()
    {
        return new BranchId(ObjectId.GenerateNewId());
    }

    public static BranchId Of(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Branch ID cannot be null or empty", nameof(id));
        }

        if (!ObjectId.TryParse(id, out var objectId))
        {
            throw new ArgumentException("Invalid ObjectId format", nameof(id));
        }

        return new BranchId(objectId);
    }

    public static BranchId Of(ObjectId objectId)
    {
        return new BranchId(objectId);
    }
}
