

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IAuditable
{
    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; init; }

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; init; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("UpdatedBy")]
    public string? UpdatedBy { get; init; }
}
