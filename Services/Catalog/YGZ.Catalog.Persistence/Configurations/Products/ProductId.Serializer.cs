

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Products;

public class ProductIdSerialzer : SerializerBase<ProductId>
{
    public override ProductId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadObjectId();
        return ProductId.CreateUnique();
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProductId value)
    {
        context.Writer.WriteObjectId(value.Value);
    }
}