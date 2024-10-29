
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace YGZ.Catalog.Persistence.Configurations.Common;

public class LocalDateTimeSerializer : SerializerBase<DateTime>
{
    private static readonly TimeZoneInfo VietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

    public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        // Read the Unix timestamp from MongoDB and convert it to a DateTime in Vietnam time
        var utcDateTime = DateTimeOffset.FromUnixTimeMilliseconds(context.Reader.ReadDateTime()).UtcDateTime;
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, VietnamTimeZone);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
    {
        // Convert the input DateTime (in Vietnam time) to UTC and then to milliseconds since Unix epoch
        var vietnamTime = TimeZoneInfo.ConvertTime(value, VietnamTimeZone);
        var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(vietnamTime, VietnamTimeZone);
        var unixTimeMilliseconds = new DateTimeOffset(utcDateTime).ToUnixTimeMilliseconds();

        context.Writer.WriteDateTime(unixTimeMilliseconds);
    }
}
