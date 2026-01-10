
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class SkuIdSerialization : SerializerBase<SkuId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SkuId value)
    {
        if (value is null || value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((ObjectId)value.Id!);
    }

    public override SkuId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull(); // Consume the null value

            return new SkuId { Id = null };
        }

        var objectId = context.Reader.ReadObjectId();

        return new SkuId { Id = objectId };
    }
}
