

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class ProductItemId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Value { get; }
    public string ValueStr { get; }

    public ProductItemId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    }

    public static ProductItemId CreateUnique()
    {
        return new(ObjectId.GenerateNewId());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
