

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;


public class ProductId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Value { get; set; }
    public string ValueStr { get; set; }

    public ProductId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    } 

    public static ProductId CreateUnique()
    {
        return new (ObjectId.GenerateNewId());
    }

    public static ObjectId? ToObjectId(string Id)
    {
        try
        {
            return !string.IsNullOrEmpty(Id) ? ObjectId.Parse(Id) : null;
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
