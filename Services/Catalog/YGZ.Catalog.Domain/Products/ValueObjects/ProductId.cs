

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;


public class ProductId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public ObjectId Value { get; }
    public string ValueStr { get; }

    public ProductId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    } 

    public static ProductId CreateUnique()
    {
        return new (ObjectId.GenerateNewId());
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
