using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class SKUCode : ValueObject
{
    public string Value { get; init; }

    private SKUCode(string value)
    {
        Value = value;
    }

    public static SKUCode Create(EProductType productType, EIphoneModel model, EStorage storage, EColor color)
    {
        return new SKUCode($"{productType.Name}-{model.Name}-{storage.Name}-{color.Name}");
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
