

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Categories;

public class CategoryIdSerializer : SerializerBase<CategoryId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CategoryId value)
    {
        if (value is null)
        {
            context.Writer.WriteNull();
            return;
        }
        context.Writer.WriteObjectId(value.Value);
    }

    public override CategoryId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        if (bsonType == BsonType.Null)
        {
            context.Reader.ReadNull();
            return null;
        }

        var objectId = context.Reader.ReadObjectId();
        return new CategoryId(objectId);
    }
}
