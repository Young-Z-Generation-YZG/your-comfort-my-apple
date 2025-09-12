
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Iphone.Entities;

[BsonCollection("SKUs")]
public class SKU : Entity<SKUId>, IAuditable, ISoftDelete
{
    public SKU(SKUId id) : base(id) { }

    [BsonElement("category_id")]
    public CategoryId CategoryId { get; set; } = default!;

    [BsonElement("sku_code")]
    public required SKUCode SKUCode { get; init; }

    [BsonElement("product_type")]
    public required EProductType ProductType { get; init; }

    [BsonElement("state")]
    public State State { get; set; } = State.INACTIVE;

    [BsonElement("model")]
    public Model Model { get; set; } = default!;

    [BsonElement("color")]
    public Color Color { get; set; } = default!;

    [BsonElement("storage")]
    public Storage Storage { get; set; } = default!;

    [BsonElement("unit_price")]
    public decimal UnitPrice { get; set; } = 0;

    [BsonElement("available_in_stock")]
    public int AvailableInStock { get; set; } = 0;

    [BsonElement("total_sold")]
    public int TotalSold { get; set; } = 0;

    [BsonElement("slug")]
    public Slug Slug { get; set; } = default!;

    public DateTime CreatedAt => DateTime.UtcNow;

    public DateTime UpdatedAt => DateTime.UtcNow;

    public string? ModifiedBy => null;

    public bool IsDeleted => false;

    public DateTime? DeletedAt => null;

    public string? DeletedBy => null;

    public static SKU Create(
        SKUCode skuCode,
        CategoryId categoryId,
        EProductType productType,
        Model model,
        Color color,
        Storage storage,
        decimal unitPrice,
        int availableInStock = 0)
    {
        var sku = new SKU(SKUId.Create())
        {
            SKUCode = skuCode,
            CategoryId = categoryId,
            ProductType = productType,
            Model = model,
            Color = color,
            Storage = storage,
            UnitPrice = unitPrice,
            AvailableInStock = availableInStock,
            Slug = Slug.Create(skuCode.Value)
        };

        return sku;
    }
}
