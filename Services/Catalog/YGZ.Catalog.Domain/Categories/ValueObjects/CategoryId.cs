

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Domain.Categories.ValueObjects;

public class CategoryId : ValueObject
{
    //[BsonId]
    [BsonRepresentation(BsonType.String)]
    public ObjectId Value { get; }
    public string ValueStr { get; }
    public CategoryId(ObjectId value)
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
        try
        {
            return !string.IsNullOrEmpty(objectId) ? new(ObjectId.Parse(objectId)) : null;
        }
        catch
        {
            return null;
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
