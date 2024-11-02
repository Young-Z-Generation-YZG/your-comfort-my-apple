
using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class InventoryId : ValueObject
{
    public ObjectId Value { get; }
    public string ValueStr { get; }

    public InventoryId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    }

    public static InventoryId CreateUnique()
    {
        return new(ObjectId.GenerateNewId());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
