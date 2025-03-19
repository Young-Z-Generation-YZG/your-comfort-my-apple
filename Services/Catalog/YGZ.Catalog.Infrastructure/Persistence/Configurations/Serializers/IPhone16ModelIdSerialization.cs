
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class IPhone16ModelIdSerialization : SerializerBase<IPhone16ModelId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IPhone16ModelId value)
    {
        if (value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override IPhone16ModelId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadObjectId();

        return new IPhone16ModelId { Id = objectId };
    }
}
