
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class ProductItemIdSerialization : SerializerBase<ProductItemId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ProductItemId value)
    {
       if(value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override ProductItemId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadObjectId();

        return new ProductItemId { Id = objectId };
    }
}
