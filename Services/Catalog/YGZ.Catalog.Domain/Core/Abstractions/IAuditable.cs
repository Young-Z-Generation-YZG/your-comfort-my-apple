

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IAuditable
{
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; }

    [BsonElement("modified_by")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ModifiedBy { get; }
}
