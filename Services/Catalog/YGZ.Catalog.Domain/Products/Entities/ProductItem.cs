
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Events;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Entities;

public class ProductItem : Entity<ProductItemId>, IAuditable
{
    [BsonElement("product_id")]
    public ProductId ProductId { get; } 

    [BsonElement("model")]
    public string Model { get; }

    [BsonElement("color")]
    public string Color { get; }

    [BsonElement("storage")]
    public int Storage { get; }

    [BsonElement("sku")]
    public SKU Sku { get; }

    [BsonElement("price")]
    public double Price { get; }

    [BsonElement("quantity_in_stock")]
    public int QuantityInStock { get; private set; }

    [BsonElement("images")]
    public List<Image> Images { get; }

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; }

    private ProductItem(ProductItemId productItemId, ProductId productId, string model, string color, int storage, SKU sku, double price, int quantityInStock, List<Image> images, DateTime created_at, DateTime updated_at) : base(productItemId)
    {
        ProductId = productId;
        Model = model;
        Color = color;
        Storage = storage;
        Sku = sku;
        Price = price;
        QuantityInStock = quantityInStock;
        Images = images;
        CreatedAt = created_at;
        UpdatedAt = updated_at;
    }

    public static ProductItem Create(ProductItemId productItemId, ProductId productId, string model, string color, int storage, double price, int quantityInStock, List<Image> images)
    {
        var sku = SKU.Create("SKU_DEMO");
        var now = DateTime.UtcNow;
        var utc = now.ToUniversalTime();

        var newProductItem = new ProductItem(productItemId, productId, model, color, storage, sku, price, quantityInStock, images, utc, utc);

        newProductItem.AddDomainEvent(new ProductItemCreatedEvent(newProductItem));

        return newProductItem;
    }
}
