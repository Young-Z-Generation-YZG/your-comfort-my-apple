

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get;}
    DateTime? DeletedAt { get; }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId? DeletedByUserId { get; }
}
