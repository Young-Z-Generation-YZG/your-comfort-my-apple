

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class CategoryIdSerialization : SerializerBase<CategoryId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CategoryId value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override CategoryId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        if (context.Reader.CurrentBsonType == BsonType.Null)
        {
            throw new InvalidCastException("Cannot convert null to CategoryId");
        }

        var objectId = context.Reader.ReadObjectId();

        return new CategoryId { Id = objectId };
    }
}
