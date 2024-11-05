
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Entities;

public class ProductItem : Entity<ProductItemId>, IAuditable
{
    public string Model { get; }
    public string Color { get; }
    public int Storage { get; }
    public SKU Sku { get; }
    public double Price { get; }
    public int QuantityInStock { get; private set; }
    public List<Image> Images { get; }

    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; set; }

    private ProductItem(ProductItemId productItemId,string model, string color, int storage, SKU sku, double price, int quantityInStock, List<Image> images, DateTime created_at, DateTime updated_at) : base(productItemId)
    {
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

    public static ProductItem Create(string model, string color, int storage, double price, int quantityInStock, List<Image> images)
    {
        var sku = SKU.Create("SKU_DEMO");
        var now = DateTime.UtcNow;
        var utc = now.ToUniversalTime();

        return new ProductItem(ProductItemId.CreateUnique(), model, color, storage, sku, price, quantityInStock, images, utc, utc);
    }
}
