
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Model : ValueObject
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("order")]
    public int Order { get; set; } = 0;

    private Model(EIphoneModel model, int order)
    {
        Name = model.Name;
        Order = order;
    }

    public static Model Create(string name, int order)
    {
        EIphoneModel.TryFromName(name, out var model);

        if (model is null)
        {
            throw new ArgumentException("Invalid EStorage ${name}", name);
        }

        return new Model(model, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Order;
    }
}
