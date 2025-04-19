

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class ReviewIdSerialization : SerializerBase<ReviewId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ReviewId value)
    {
        if (value is null || value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override ReviewId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull(); // Consume the null value
            return new ReviewId { Id = null };
        }

        var objectId = context.Reader.ReadObjectId();

        return new ReviewId { Id = objectId };
    }
}
