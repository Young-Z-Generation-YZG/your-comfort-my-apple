

using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Categories.ValueObjects;

public class CategoryId : ValueObject
{
    public ObjectId Value { get; }
    public string ValueStr => Value.ToString();

    private CategoryId(ObjectId value)
    {
        Value = value;
    }

    public static CategoryId Create()
    {
        return new(ObjectId.GenerateNewId());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
