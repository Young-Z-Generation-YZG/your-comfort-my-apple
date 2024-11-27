
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Events;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Entities;

public class ProductItem : Entity<ProductItemId>, IInventory, IAuditable, ISoftDelete
{
    [BsonElement("productId")]
    public ProductId ProductId { get; private set; } 

    [BsonElement("sku")]
    public SKU Sku { get; private set; }

    [BsonElement("model")]
    public string Model { get; private set; }

    [BsonElement("color")]
    public string Color { get; private set; }

    [BsonElement("storage")]
    public int Storage { get; private set; }

    [BsonElement("price")]
    public double Price { get; private set; }

    [BsonElement("quantity_remain")]
    public int QuantityRemain { get; set; }

    [BsonElement("quantity_in_stock")]
    public int QuantityInStock { get; set; } 

    [BsonElement("sold")]
    public int Sold { get; set; } = 0;

    [BsonElement("images")]
    public List<Image> Images { get; private set; }

    [BsonElement("state")]
    public ProductStateEnum State { get; private set; } = ProductStateEnum.INACTIVE;

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("is_deleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt { get; set; } = null;

    [BsonElement("deleted_by")]
    public string? DeletedByUserId { get; set; } = null;
   

    private ProductItem(
        ProductItemId productItemId,
        ProductId productId,
        string model,
        string color,
        int storage,
        SKU sku,
        double price,
        int quantityInStock,
        List<Image> images,
        DateTime createdAt,
        DateTime updatedAt) : base(productItemId)
    {
        ProductId = productId;
        Model = model;
        Color = color;
        Storage = storage;
        Sku = sku;
        Price = price;
        QuantityInStock = quantityInStock;
        QuantityRemain = quantityInStock;
        Images = images;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ProductItem Create(ProductItemId productItemId, ProductId productId, string model, string color, int storage, double price, int quantityInStock, List<Image> images, DateTime createdDate)
    {
        var sku = SKU.Create("SKU_DEMO");
        //var now = DateTime.UtcNow;
        //var utc = now.ToUniversalTime();

        var newProductItem = new ProductItem(productItemId, productId, model, color, storage, sku, price, quantityInStock, images, createdDate, createdDate);

        newProductItem.AddDomainEvent(new ProductItemCreatedEvent(newProductItem));

        return newProductItem;
    }
}
