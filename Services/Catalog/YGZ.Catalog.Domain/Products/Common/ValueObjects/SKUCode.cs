using YGZ.BuildingBlocks.Shared.Utils;
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
        EIphoneModel.TryFromName(SnakeCaseSerializer.Serialize(model).ToUpper(), out var modelEnum);
        EStorage.TryFromName(storage, out var storageEnum);
        EColor.TryFromName(SnakeCaseSerializer.Serialize(color).ToUpper(), out var colorEnum);

        if (productTypeEnum is null || modelEnum is null || storageEnum is null || colorEnum is null)
        {
            throw new ArgumentException("Invalid product type, model, storage or color");
        }

        return new SKUCode($"{productTypeEnum.Name}-{modelEnum.Name}-{storageEnum.Name}-{colorEnum.Name}");
    }

    public static SKUCode Of(string value)
    {
        var parts = value.Split('-');
        if (parts.Length != 4)
        {
            throw new ArgumentException("Invalid SKU code format");
        }

        return Create(parts[0], parts[1], parts[2], parts[3]);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
