using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;

public class SkuPriceList : ValueObject
{
    [BsonElement("normalized_model")]
    public required string NormalizedModel { get; init; }

    [BsonElement("normalized_color")]
    public required string NormalizedColor { get; init; }

    [BsonElement("normalized_storage")]
    public required string NormalizedStorage { get; init; }

    [BsonElement("unit_price")]
    public required decimal UnitPrice { get; init; }

    public static SkuPriceList Create(string model, string color, string storage, decimal unitPrice)
    {
        EIphoneModel.TryFromName(model, out var modelEnum);
        EColor.TryFromName(color, out var colorEnum);
        EStorage.TryFromName(storage, out var storageEnum);

        return new SkuPriceList
        {
            NormalizedModel = modelEnum.Name,
            NormalizedColor = colorEnum.Name,
            NormalizedStorage = storageEnum.Name,
            UnitPrice = unitPrice
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return NormalizedModel;
        yield return NormalizedColor;
        yield return NormalizedStorage;
        yield return UnitPrice;
    }

    public SkuPriceListResponse ToResponse()
    {
        return new SkuPriceListResponse
        {
            NormalizedModel = NormalizedModel,
            NormalizedColor = NormalizedColor,
            NormalizedStorage = NormalizedStorage,
            UnitPrice = UnitPrice
        };
    }
}