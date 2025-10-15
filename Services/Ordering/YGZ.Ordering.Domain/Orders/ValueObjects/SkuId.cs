using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class SkuId : ValueObject
{
    public string Value { get; private set; }


    private SkuId(string id)
    {
        Value = id;
    }

    public static SkuId Of(string skuId)
    {
        return new SkuId(skuId);
    }


    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

