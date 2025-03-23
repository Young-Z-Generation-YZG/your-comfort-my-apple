
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

public class IPhone16ModelId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static IPhone16ModelId Create()
    {
        return new IPhone16ModelId { Id = ObjectId.GenerateNewId() };
    }

    public static IPhone16ModelId Of(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        ObjectId.TryParse(id, out var value);

        ArgumentException.ThrowIfNullOrWhiteSpace(value.ToString());

        return new IPhone16ModelId { Id = value };
    }
}
