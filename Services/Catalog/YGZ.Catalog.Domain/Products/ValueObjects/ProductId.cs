

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class ProductId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static ProductId Create()
    {
        return new ProductId { Id = ObjectId.GenerateNewId() };
    }

    public static ProductId ToValueObjectId(string? id)
    {
        var isParse = ObjectId.TryParse(id, out _);

        return new ProductId { Id = isParse ? ObjectId.Parse(id) : null };
    }
}
