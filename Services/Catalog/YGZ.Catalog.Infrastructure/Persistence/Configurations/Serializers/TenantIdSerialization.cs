using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

internal class TenantIdSerialization : SerializerBase<TenantId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TenantId value)
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

    public override TenantId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        
        switch (bsonType)
        {
            case BsonType.ObjectId:
                var objectId = context.Reader.ReadObjectId();
                return new TenantId { Id = objectId };
            case BsonType.Null:
                context.Reader.ReadNull();
                return new TenantId { Id = null };
            default:
                throw new BsonSerializationException($"Cannot deserialize a {bsonType} to TenantId.");
        }
    }
}
