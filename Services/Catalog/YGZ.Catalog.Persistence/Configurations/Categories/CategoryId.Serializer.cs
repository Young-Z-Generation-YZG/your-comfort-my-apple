

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Categories.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Categories;

public class CategoryIdSerializer : SerializerBase<CategoryId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, CategoryId value)
    {
        if(value is null)
        {
            context.Writer.WriteNull();
            return;
        }

        context.Writer.WriteObjectId(value.Value);
    }
}
