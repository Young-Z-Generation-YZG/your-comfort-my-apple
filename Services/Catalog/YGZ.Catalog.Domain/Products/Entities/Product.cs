
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace YGZ.Catalog.Domain.Products.Entities;

public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Name { get; set; } = default!;
}
