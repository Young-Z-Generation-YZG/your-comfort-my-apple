
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class ProductItemId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static ProductItemId Create()
    {
        return new ProductItemId { Id = ObjectId.GenerateNewId() };
    }

    public static ProductItemId ToValueObjectId(string? id)
    {
        var isParse = ObjectId.TryParse(id, out _);

        return new ProductItemId { Id = isParse ? ObjectId.Parse(id) : null };
    }
}
