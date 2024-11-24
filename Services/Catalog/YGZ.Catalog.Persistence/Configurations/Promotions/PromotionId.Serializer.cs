
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using YGZ.Catalog.Domain.Promotions.ValueObjects;

namespace YGZ.Catalog.Persistence.Configurations.Promotions;

public class PromotionIdSerialzier : SerializerBase<PromotionId>
{
    //public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, PromotionId value)
    //{
    //    if (value is null)
    //    {
    //        context.Writer.WriteNull();
    //        return;
    //    }

    //    context.Writer.WriteObjectId(value.Value);
    //}

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, PromotionId value)
        {
            if (value is null)
            {
                context.Writer.WriteNull();
                return;
            }

            context.Writer.WriteObjectId(value.Value);
        }

        public override PromotionId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();
            if (bsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return null;
            }

            var objectId = context.Reader.ReadObjectId();
            return new PromotionId(objectId);
        }
}
