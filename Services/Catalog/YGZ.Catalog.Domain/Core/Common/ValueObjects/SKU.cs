
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Common.ValueObjects;


public class SKU : ValueObject
{
    public string Value { get; }

    private SKU(string value)
    {
        Value = value;
    }

    public static SKU Create(string value)
    {
        return new SKU(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
