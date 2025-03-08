

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class ProductIdSerialization : SerializerBase<ProductId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProductId value)
    {
        if (value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override ProductId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadObjectId();

        return new ProductId { Id = objectId };
    }
}
