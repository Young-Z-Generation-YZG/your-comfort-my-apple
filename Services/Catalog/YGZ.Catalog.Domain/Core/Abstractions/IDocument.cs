

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    ObjectId Id { get; set; }
}
