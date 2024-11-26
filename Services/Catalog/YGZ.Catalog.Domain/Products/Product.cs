
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Entities;
using YGZ.Catalog.Domain.Products.ValueObjects;
using YGZ.Catalog.Domain.Promotions;
using YGZ.Catalog.Domain.Promotions.ValueObjects;

namespace YGZ.Catalog.Domain.Products;

public class Product : AggregateRoot<ProductId>, IAuditable
{
    private readonly List<ProductItem> _productItems = new();
    private readonly Category _category;
    private readonly Promotion _promotion;
    public readonly AverageRating _averageRating;

    [BsonElement("categoryId")]
    public CategoryId CategoryId { get; private set; }

     [BsonElement("promotionId")]
    public PromotionId PromotionId { get; private set; }

    [BsonElement("name")]
    public string Name { get; private set; }

    [BsonElement("models")]
    public List<Model> Models { get; private set; }

    [BsonElement("colors")]
    public List<Color> Colors { get; private set; } = new();

    [BsonElement("storages")]
    public List<StorageEnum> Storages { get; private set; }

    [BsonElement("description")]
    public string Description { get; private set; } = "";

    [BsonElement("average_rating")]
    public AverageRating AverageRating { get; set; } = AverageRating.CreateNew();

    [BsonElement("star_ratings")]
    public List<StarRating> StarRatings { get; private set; } = StarRating.CreateNewList(5).ToList();

    [BsonElement("images")]
    public List<Image> Images { get; private set; }

    [BsonElement("product_items")]
    public List<ProductItem> ProductItems { get; private set; } = new();

    [BsonElement("state")]
    public ProductStateEnum State { get; private set; } = ProductStateEnum.INACTIVE;

    [BsonElement("slug")]
    public Slug Slug { get; private set; }

    [BsonElement("created_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updated_at")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }

    private Product(
        ProductId productId,
        string name,
        List<Model> models,
        List<Color> colors,
        List<StorageEnum> storages,
        string description,
        List<Image> images,
        List<ProductItem> productItems,
        Slug slug,
        CategoryId categoryId,
        PromotionId promotionId,
        DateTime createdAt) : base(productId)
    {
        Name = name;
        Models = models;
        Colors = colors;
        Storages = storages;
        Description = description;
        Images = images;
        _productItems = productItems;
        Slug = slug;
        CategoryId = categoryId;
        PromotionId = promotionId;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Product Create(
        ProductId productId,
        CategoryId? categoryId,
        PromotionId? promotionId,
        string name,
        List<Model> models,
        List<Color> colors,
        List<StorageEnum> storages,
        string? description,
        List<Image> images,
        DateTime createdAt)
    {

        var product = new Product(
                           productId,
                           name,
                           models,
                           colors,
                           storages,
                           description ?? "",
                           images,
                           new(),
                           Slug.Create(name),
                           categoryId!,
                           promotionId!,
                           createdAt);

        return product;
    }

    public List<ProductItem> AddProductItem(ProductItem productItem)
    {
        //var newItem = ProductItem.Create(productItem.Id, productItem.ProductId, productItem.Model, productItem.Color, productItem.Storage, productItem.Price, productItem.QuantityInStock, productItem.Images);

        _productItems.Add(productItem);

        return _productItems;
    }

    public string GetSlug => Slug.Value;
}
