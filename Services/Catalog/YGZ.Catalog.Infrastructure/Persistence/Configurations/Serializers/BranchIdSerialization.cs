using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

internal class BranchIdSerialization : SerializerBase<BranchId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, BranchId value)
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

    public override BranchId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();
        
        switch (bsonType)
        {
            case BsonType.ObjectId:
                var objectId = context.Reader.ReadObjectId();
                return new BranchId { Id = objectId };
            case BsonType.Null:
                context.Reader.ReadNull();
                return new BranchId { Id = null };
            default:
                throw new BsonSerializationException($"Cannot deserialize a {bsonType} to BranchId.");
        }
    }
}
