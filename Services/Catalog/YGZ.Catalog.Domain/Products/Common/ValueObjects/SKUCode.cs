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

    public static SKUCode Create(string productType, string model, string storage, string color)
    {
        EProductType.TryFromName(productType, out var productTypeEnum);
        EIphoneModel.TryFromName(model, out var modelEnum);
        EStorage.TryFromName(storage, out var storageEnum);
        EColor.TryFromName(color, out var colorEnum);

        if (productTypeEnum is null || modelEnum is null || storageEnum is null || colorEnum is null)
        {
            throw new ArgumentException("Invalid product type, model, storage or color");
        }

        return new SKUCode($"{productTypeEnum.Name}-{modelEnum.Name}-{storageEnum.Name}-{colorEnum.Name}");
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
