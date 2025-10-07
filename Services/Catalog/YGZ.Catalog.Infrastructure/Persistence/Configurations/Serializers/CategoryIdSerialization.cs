

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class CategoryIdSerialization : SerializerBase<CategoryId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CategoryId value)
    {
        if (value?.Id == null)
        {
            context.Writer.WriteNull();
        }
        else
        {
            context.Writer.WriteObjectId(value.Id.Value);
        }
    }

    public override CategoryId? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();

        if (bsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null;
        }

        var objectId = context.Reader.ReadObjectId();
        return new CategoryId { Id = objectId };
    }
}
