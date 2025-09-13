
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class ModelBK : ValueObject
{
    [BsonElement("model_name")]
    public string ModelName { get; set; }

    [BsonElement("model_order")]
    public int ModelOrder { get; set; } = 0;

    private ModelBK(string name, int order)
    {
        ModelName = name;
        ModelOrder = order;
    }

    public static ModelBK Create(string modelName, int modelOrder)
    {
        EIphoneModel.TryFromName(modelName, out var iPhoneModel);

        return new ModelBK(iPhoneModel.Name, modelOrder);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ModelName;
        yield return ModelOrder;
    }
}
