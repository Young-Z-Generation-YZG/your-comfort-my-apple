
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;
using MongoDB.Bson;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class IPhone16IdSerialization : SerializerBase<IPhone16Id>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, IPhone16Id value)
    {
        if (value is null || value.Id is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override IPhone16Id Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            context.Reader.ReadNull(); // Consume the null value

            return new IPhone16Id { Id = null };
        }

        var objectId = context.Reader.ReadObjectId();

        return new IPhone16Id { Id = objectId };
    }
}
