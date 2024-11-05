//using MongoDB.Bson.Serialization.Serializers;
//using MongoDB.Bson.Serialization;
//using YGZ.Catalog.Domain.Products.ValueObjects;
//using YGZ.Catalog.Domain.Core.Primitives;
//using MongoDB.Bson;

//namespace YGZ.Catalog.Persistence.Configurations.Products;

//public class ObjectIdSerialzer<T> : SerializerBase<T> where T : ValueObject
//{
//    //public override ProductId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
//    //{
//    //    var value = context.Reader.ReadObjectId();
//    //    return ProductId.CreateUnique();
//    //}

//    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
//    {
//        context.Writer.WriteObjectId(value.Value);
//    }
//}