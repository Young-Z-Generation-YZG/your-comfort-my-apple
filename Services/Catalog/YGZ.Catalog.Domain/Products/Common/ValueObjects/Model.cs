
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Model : ValueObject
{
    [BsonElement("model_name")]
    public string ModelName { get; set; }

    [BsonElement("model_order")]
    public int? ModelOrder { get; set; }

    private Model(string name, int? order)
    {
        ModelName = name;
        ModelOrder = order;
    }

    public static Model Create(string modelName, int? modelOrder)
    {
        return new Model(modelName, modelOrder);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ModelName;
        yield return ModelOrder;
    }
}
