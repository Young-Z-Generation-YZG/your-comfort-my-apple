

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories.ValueObjects;

public class CategoryId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; init; }
    public string? Value => Id.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }

    public static CategoryId Create()
    {
        return new CategoryId { Id = ObjectId.GenerateNewId() };
    }

    public static CategoryId Of(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var isSuccess = ObjectId.TryParse(id, out var objectId);

        if (!isSuccess) throw new InvalidCastException($"Cannot convert id:{id} to CategoryId");

        return new CategoryId { Id = objectId };
    }
}