
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

public class ReviewId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static ReviewId Create()
    {
        return new ReviewId { Id = ObjectId.GenerateNewId() };
    }

    public static ReviewId Of(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        ObjectId.TryParse(id, out var value);

        ArgumentException.ThrowIfNullOrWhiteSpace(value.ToString());

        return new ReviewId { Id = value };
    }
}
