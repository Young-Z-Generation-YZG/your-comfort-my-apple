using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;

public class RequestId : ValueObject
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; }
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static RequestId Create()
    {
        return new RequestId { Id = ObjectId.GenerateNewId() };
    }

    public static RequestId Of(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        if (ObjectId.TryParse(id, out var value))
        {
            return new RequestId { Id = value };
        }

        throw new ArgumentException("Invalid ObjectId format", nameof(id));
    }
}
