
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Products;

public class ProductItemIdSerialzer : SerializerBase<ProductItemId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProductItemId value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }

        context.Writer.WriteObjectId(value.Value);
    }
}
