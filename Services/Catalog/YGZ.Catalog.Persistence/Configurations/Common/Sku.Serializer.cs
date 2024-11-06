

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Common;

class SKUSerializer : SerializerBase<SKU>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SKU value)
    {
        context.Writer.WriteString(value.Value);
    }
}
