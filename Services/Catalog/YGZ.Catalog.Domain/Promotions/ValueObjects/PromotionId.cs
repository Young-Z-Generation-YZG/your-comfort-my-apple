

using MongoDB.Bson;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Promotions.ValueObjects;

public class PromotionId : ValueObject
{
    public ObjectId Value { get; }
    public string ValueStr { get; }
    
    public PromotionId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    }

    public static PromotionId Create()
    {
        return new(ObjectId.GenerateNewId());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
