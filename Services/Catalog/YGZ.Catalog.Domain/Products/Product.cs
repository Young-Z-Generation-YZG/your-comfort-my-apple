
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
    public readonly AverageRating _averageRating;

    [BsonElement("name")]
    public string Name { get; private set; }

     [BsonElement("description")]
    public string Description { get; private set; }

     [BsonElement("images")]
    public List<Image> Images { get; private set; }

    [BsonElement("average_rating_value")]
    public double AverageRatingValue { get; private set; }

    [BsonElement("average_rating_num_ratings")]
    public int AverageRatingNumRatings { get; private set; }

    [BsonElement("slug")]
    public Slug Slug { get; private set; }

    [BsonElement("product_items")]
    public List<ProductItem> ProductItems { get; private set; } = new();

    [BsonElement("models")]
    public List<string> Models { get; private set; } = new();

    [BsonElement("colors")]
    public List<string> Colors { get; private set; } = new();

    [BsonElement("CategoryId")]
    public CategoryId CategoryId { get; private set; }

     [BsonElement("promotionId")]
    public PromotionId PromotionId { get; private set; }

     [BsonElement("created_at", Order = 1)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

     [BsonElement("updated_at", Order = 2)]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }

    private Product(
        ProductId productId,
        string name,
        Slug slug,
        string description,
        List<Image> images,
        AverageRating averageRating,
        List<ProductItem> product_items,
        List<string> models,
        List<string> colors,
        CategoryId categoryId,
        PromotionId promotionId,
        DateTime created_at,
        DateTime updated_at) : base(productId)
    {
        Name = name;
        Description = description;
        Images = images;
        _averageRating = averageRating;
        AverageRatingValue = averageRating.Value;
        AverageRatingNumRatings = averageRating.NumRatings;
        Slug = slug;
        _productItems = product_items;
        Models = models;
        Colors = colors;
        CategoryId = categoryId;
        PromotionId = promotionId;
        CreatedAt = created_at;
        UpdatedAt = updated_at;
    }

    public static Product Create(ProductId productId, string name, string? description, List<Image> images, double valueRating, int numsRating, List<string> models,List<string> colors, CategoryId? categoryId, PromotionId? promotionId)
    {
        var now = DateTime.UtcNow;
        var utc = now.ToUniversalTime();

        var product = new Product(productId,
                           name,
                           Slug.Create(name),
                           description ?? "",
                           images,
                           AverageRating.CreateNew(valueRating, numsRating),
                           [],
                           models,
                           colors,
                           categoryId!,
                           promotionId!,
                           utc,
                           utc); 

        return product;
    }

    public List<ProductItem> AddProductItem(ProductItem productItem)
    {
        var newItem = ProductItem.Create(productItem.Id, productItem.ProductId, productItem.Model, productItem.Color, productItem.Storage, productItem.Price, productItem.QuantityInStock, productItem.Images);

        _productItems.Add(newItem);

        return _productItems;
    }
}
