

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Common;

public class SlugSerializer : SerializerBase<Slug>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Slug value)
    {
        context.Writer.WriteString(value.Value);
    }

    public override Slug Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadString();
        return Slug.Create(value);
    }

}
