using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class SkuCode : ValueObject
{
    public string Value { get; init; }

    private SkuCode(string value)
    {
        Value = value;
    }

    public static SkuCode Create(string productClassification, string model, string storage, string color)
    {
        EProductClassification.TryFromName(productClassification, out var productClassificationEnum);
        EIphoneModel.TryFromName(SnakeCaseSerializer.Serialize(model).ToUpper(), out var modelEnum);
        EStorage.TryFromName(storage, out var storageEnum);
        EColor.TryFromName(SnakeCaseSerializer.Serialize(color).ToUpper(), out var colorEnum);

        if (productClassificationEnum is null || modelEnum is null || storageEnum is null || colorEnum is null)
        {
            throw new ArgumentException("Invalid product classification, model, storage or color");
        }

        return new SkuCode($"{productClassificationEnum.Name}-{modelEnum.Name}-{storageEnum.Name}-{colorEnum.Name}");
    }

    public static SkuCode Of(string value)
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
