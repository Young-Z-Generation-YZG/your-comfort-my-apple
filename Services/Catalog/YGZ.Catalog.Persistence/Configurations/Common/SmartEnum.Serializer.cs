

using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using Ardalis.SmartEnum;
using MongoDB.Bson.IO;
using MongoDB.Bson;

namespace YGZ.Catalog.Persistence.Configurations.Common;

public class SmartEnumSerializer<TEnum> : SerializerBase<TEnum> where TEnum : SmartEnum<TEnum>
{
    public override TEnum Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.GetCurrentBsonType();

        if (bsonType == BsonType.Document)
        {
            context.Reader.ReadStartDocument();
            string? name = null;
            int? value = null;

            while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var fieldName = context.Reader.ReadName();

                if (fieldName == "Name")
                {
                    name = context.Reader.ReadString();
                }
                else if (fieldName == "Value")
                {
                    value = context.Reader.ReadInt32();
                }
            }

            context.Reader.ReadEndDocument();

            if (value.HasValue)
            {
                return SmartEnum<TEnum>.FromValue(value.Value);
            }

            throw new BsonSerializationException("Missing 'Value' field for SmartEnum deserialization.");
        }

        throw new BsonSerializationException($"Unexpected BSON type '{bsonType}' for SmartEnum.");
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, TEnum value)
    {
        context.Writer.WriteStartDocument();
        context.Writer.WriteName("Name");
        context.Writer.WriteString(value.Name);
        context.Writer.WriteName("Value");
        context.Writer.WriteInt32(value.Value);
        context.Writer.WriteEndDocument();
    }
}