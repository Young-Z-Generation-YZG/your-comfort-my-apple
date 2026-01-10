using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class RequestIdSerialization : SerializerBase<RequestId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RequestId value)
    {
        if (value is null || value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((ObjectId)value.Id!);
    }

    public override RequestId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull(); // Consume the null value

            return new RequestId { Id = null };
        }

        var objectId = context.Reader.ReadObjectId();

        return new RequestId { Id = objectId };
    }
}
