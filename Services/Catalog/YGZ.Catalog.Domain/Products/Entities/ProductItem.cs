
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
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
    required public string Model { get; set; }

    [BsonElement("color")]
    required public Color Color { get; set; }

    [BsonElement("storage")]
    required public StorageEnum Storage { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; } = 0;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("available_in_stock")]
    public int AvailableInStock { get; set; } = 0;

    [BsonElement("sold")]
    public int Sold { get; set; } = 0;

    [BsonElement("state")]
    public StateEnum State { get; set; } = StateEnum.INACTIVE;

    [BsonElement("average_rating")]
    public AverageRating AverageRating { get; set; } = AverageRating.CreateNew();

    [BsonElement("images")]
    public Image[] Images { get; set; } = [];

    [BsonElement("product_id")]
    required public ProductId ProductId { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt => Id?.Id!.Value.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id?.Id!.Value.CreationTime ?? DateTime.Now;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by_user_id")]
    public string? DeletedByUserId => null;

    public static ProductItem Create(string model, Color color, int storage, decimal price, string description,Image[] images, string productId)
    {
        var storageEnum = StorageEnum.FromValue(storage);

        return new ProductItem(ProductItemId.Create())
        {
            Model = model,
            Color = color,
            Storage = storageEnum,
            Price = price,
            Description = description,
            Images = images,
            ProductId = ProductId.ToValueObjectId(productId)
        };
    }
}
