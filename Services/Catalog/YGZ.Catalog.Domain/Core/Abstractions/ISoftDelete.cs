

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface ISoftDelete
{
    [BsonElement("is_deleted")]
    bool IsDeleted { get; }

    [BsonElement("deleted_at")]
    DateTime? DeletedAt { get; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("deleted_by")]
    string? DeletedBy { get; }
}
