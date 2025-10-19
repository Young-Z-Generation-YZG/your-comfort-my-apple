using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Storage : ValueObject
{
    [BsonElement("name")]
    public string Name { get; init; }

    [BsonElement("normalized_name")]
    public string NormalizedName { get; set; }

    [BsonElement("value")]
    public int Value { get; init; }

    [BsonElement("order")]
    public int Order { get; init; } = 0;

    private Storage(string normalizedName, EStorage storageEnum, int value, int order)
    {
        Name = storageEnum.Name;
        NormalizedName = normalizedName;
        Value = value;
        Order = order;
    }

    public static Storage Create(string name, int value, int order)
    {
        var normalizedName = SnakeCaseSerializer.Serialize(name).ToUpper();

        EStorage.TryFromName(normalizedName, out var storageEnum);

        if (storageEnum is null)
        {
            throw new ArgumentException("Invalid EStorage ${name}", name);
        }

        return new Storage(normalizedName, storageEnum, value, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
        yield return Order;
    }

    public StorageResponse ToResponse()
    {
        return new StorageResponse
        {
            Name = Name,
            NormalizedName = NormalizedName,
            Order = Order
        };
    }
}
