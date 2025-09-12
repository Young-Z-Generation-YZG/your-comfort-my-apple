
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

public class IphoneModelId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static IphoneModelId Create()
    {
        return new IphoneModelId { Id = ObjectId.GenerateNewId() };
    }

    public static IphoneModelId Of(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        ObjectId.TryParse(id, out var value);

        ArgumentException.ThrowIfNullOrWhiteSpace(value.ToString());

        return new IphoneModelId { Id = value };
    }
}
