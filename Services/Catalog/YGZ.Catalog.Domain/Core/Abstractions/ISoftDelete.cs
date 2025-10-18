

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface ISoftDelete
{
    [BsonElement("IsDeleted")]
    bool IsDeleted { get; init; }

    [BsonElement("DeletedAt")]
    DateTime? DeletedAt { get; init; }

    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("DeletedBy")]
    string? DeletedBy { get; init; }
}
