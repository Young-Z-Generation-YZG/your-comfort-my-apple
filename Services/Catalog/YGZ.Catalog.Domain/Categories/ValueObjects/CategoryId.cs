

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories.ValueObjects;

public class CategoryId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static CategoryId Create()
    {
        return new CategoryId { Id = ObjectId.GenerateNewId() };
    }

    public static CategoryId ToId(string? id) 
    {
        var isParse = ObjectId.TryParse(id, out _);

        return new CategoryId { Id = isParse ? ObjectId.Parse(id) : null };
    }
}
