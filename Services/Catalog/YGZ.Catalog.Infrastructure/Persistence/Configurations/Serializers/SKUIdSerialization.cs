
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class SKUIdSerialization : SerializerBase<SKUId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SKUId value)
    {
        if (value is null || value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override SKUId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull(); // Consume the null value

            return new SKUId { Id = null };
        }

        var objectId = context.Reader.ReadObjectId();

        return new SKUId { Id = objectId };
    }
}
