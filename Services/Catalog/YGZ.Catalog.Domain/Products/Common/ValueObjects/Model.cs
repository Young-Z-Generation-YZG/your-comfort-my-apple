
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Model : ValueObject
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("normalized_name")]
    public string NormalizedName { get; set; }

    [BsonElement("order")]
    public int Order { get; set; } = 0;

    private Model(string name, EIphoneModel eModel, int order)
    {
        Name = name;
        NormalizedName = eModel.Name;
        Order = order;
    }

    public static Model Create(string name, int order)
    {
        EIphoneModel.TryFromName(SnakeCaseSerializer.Serialize(name).ToUpper(), out var modelEnum);

        if (modelEnum is null)
        {
            throw new ArgumentException("Invalid EIphoneModel ${name}", name);
        }

        return new Model(name, modelEnum, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Order;
    }

    public ModelResponse ToResponse()
    {
        return new ModelResponse
        {
            Name = Name,
            NormalizedName = NormalizedName,
            Order = Order
        };
    }
}
