
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace YGZ.Catalog.Domain.Core.Base;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [BsonElement("Created_date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Created_date { get; set; } = DateTime.UtcNow;

    [BsonElement("Updated_date")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Updated_date { get; set; } = DateTime.UtcNow;
}
