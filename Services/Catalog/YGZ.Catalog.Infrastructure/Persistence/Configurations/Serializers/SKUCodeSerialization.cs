
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class SKUCodeSerialization : SerializerBase<SkuCode>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SkuCode skuCode)
    {
        if (string.IsNullOrEmpty(skuCode.Value))
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteString(skuCode.Value);
    }

    public override SkuCode Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();

        return SkuCode.Of(value);
    }
}
