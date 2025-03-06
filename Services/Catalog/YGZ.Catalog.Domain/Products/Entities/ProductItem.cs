
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Entities;

[BsonCollection("ProductItems")]
public class ProductItem : Entity<ProductItemId>, IAuditable, ISoftDelete
{
    public ProductItem(ProductItemId id) : base(id)
    {
            
    }

    [BsonElement("model")]
    public string? Model { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt => Id?.Id!.Value.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id?.Id!.Value.CreationTime ?? DateTime.Now;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by_user_id")]
    public ObjectId? DeletedByUserId => null;

    public static ProductItem Create(string model)
    {
        return new ProductItem(ProductItemId.Create())
        {
            Model = model
        };
    }
}
