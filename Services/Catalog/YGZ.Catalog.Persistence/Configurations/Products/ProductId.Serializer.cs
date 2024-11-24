

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Products;

public class ProductIdSerialzer : SerializerBase<ProductId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProductId value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }

        context.Writer.WriteObjectId(value.Value);
    }

    public override ProductId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadObjectId();
        return new ProductId(objectId);
    }
}