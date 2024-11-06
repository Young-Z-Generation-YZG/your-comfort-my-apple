
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Domain.Products.Events;
using YGZ.Catalog.Domain.Products.ValueObjects;
using YGZ.Catalog.Domain.Promotions;
using YGZ.Catalog.Domain.Promotions.ValueObjects;

namespace YGZ.Catalog.Domain.Products;

public sealed class Product : AggregateRoot<ProductId>, IAuditable
{
    private readonly List<ProductItem> _productItems = new();
    private readonly Category _category;
    private readonly Promotion _promotion;

    [BsonElement("name")]
    public string Name { get; }

    [BsonElement("description")]
    public string Description { get; }

    [BsonElement("images")]
    public List<Image> Images { get; }

    [BsonElement("average_rating")]
    public AverageRating AverageRating { get; }

    [BsonElement("slug")]
    public Slug Slug { get; }

    [BsonElement("category_id")]
    public CategoryId CategoryId { get; set; }

    [BsonElement("promotion_id")]
    public PromotionId PromotionId { get; set; }


    [BsonElement("created_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get;}

    [BsonElement("updated_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get ; set; }

    public IReadOnlyList<ProductItem> ProductItems => _productItems.AsReadOnly();

    private Product(ProductId productId, string name, Slug slug, string description, List<Image> images, AverageRating averageRating, List<ProductItem> product_items, CategoryId categoryId, PromotionId promotionId, DateTime created_at, DateTime updated_at) : base(productId)
    {
        Name = name;
        Description = description;
        Images = images;
        AverageRating = averageRating;
        Slug = slug;
        _productItems = product_items;
        CategoryId = categoryId;
        PromotionId = promotionId;
        CreatedAt = created_at;
        UpdatedAt = updated_at;
    }

    public static Product Create(string name, string? description, List<Image> images, double valueRating, int numsRating, List<ProductItem> productItems, CategoryId? categoryId, PromotionId? promotionId)
    {
        var now = DateTime.UtcNow;
        var utc = now.ToUniversalTime();

        var product = new Product(ProductId.CreateUnique(),
                           name,
                           Slug.Create(name),
                           description ?? "",
                           images,
                           AverageRating.CreateNew(valueRating, numsRating),
                           productItems,
                           categoryId!,
                           promotionId!,
                           utc,
                           utc); 

        product.AddDomainEvent(new ProductCreatedEvent(product));

        return product;
    }
}
