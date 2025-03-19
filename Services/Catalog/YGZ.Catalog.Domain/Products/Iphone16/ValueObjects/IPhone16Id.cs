

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

public class IPhone16Id : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId? Id { get; set; } = null;
    public string? Value => Id?.ToString();

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id!;
    }

    public static IPhone16Id Create()
    {
        return new IPhone16Id { Id = ObjectId.GenerateNewId() };
    }

    public static IPhone16Id Of(string? id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var isParse = ObjectId.TryParse(id, out var value);

        ArgumentException.ThrowIfNullOrWhiteSpace(value.ToString());

        return new IPhone16Id { Id = value };
    }


}
