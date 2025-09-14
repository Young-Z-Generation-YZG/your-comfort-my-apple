
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class ModelIdSerialization : SerializerBase<ModelId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ModelId value)
    {
        if (value is null || value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override ModelId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull(); // Consume the null value

            return new ModelId { Id = null };
        }

        var objectId = context.Reader.ReadObjectId();

        return new ModelId { Id = objectId };
    }
}
