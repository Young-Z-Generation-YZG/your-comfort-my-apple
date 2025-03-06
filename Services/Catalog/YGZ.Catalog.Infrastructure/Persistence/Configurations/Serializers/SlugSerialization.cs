
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

public class SlugSerialization : SerializerBase<Slug>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Slug slug)
    {
        if (string.IsNullOrEmpty(slug.Value))
        {
            context.Writer.WriteNull();

            return;
        }

        context.Writer.WriteString(slug.Value);
    }

    public override Slug Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var slug = context.Reader.ReadString();

        return new Slug { Value = slug };
    }
}
