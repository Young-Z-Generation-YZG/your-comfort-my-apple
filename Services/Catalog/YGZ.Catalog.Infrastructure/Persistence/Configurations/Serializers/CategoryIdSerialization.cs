

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class CategoryIdSerialization : SerializerBase<CategoryId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CategoryId value)
    {
        if (value.Id is null)
        {
            context.Writer.WriteNull();
            return;
        }

        context.Writer.WriteObjectId((MongoDB.Bson.ObjectId)value.Id!);
    }

    public override CategoryId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadObjectId();

        return new CategoryId { Id = objectId };
    }
}
