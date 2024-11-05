

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories.ValueObjects;

public class CategoryId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Value { get; }
    public string ValueStr { get; }
    private CategoryId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    }

    public static CategoryId CreateNew()
    {
        return new(ObjectId.GenerateNewId());
    }

    public static CategoryId? ToObjectId(string? objectId)
    {
        return !string.IsNullOrEmpty(objectId) ? new(ObjectId.Parse(objectId)) : null;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
