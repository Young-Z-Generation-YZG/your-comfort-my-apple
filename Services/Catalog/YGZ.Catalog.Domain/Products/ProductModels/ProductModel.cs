using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;

namespace YGZ.Catalog.Domain.Products.ProductModels;

[BsonCollection("ProductModels")]
public class ProductModel : AggregateRoot<ModelId>, IAuditable, ISoftDelete
{
    public ProductModel(ModelId id) : base(id)
    {

    }

    [BsonElement("category")]
    public required Category Category { get; init; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("normalized_model")]
    public required string NormalizedModel { get; init; }

    [BsonElement("product_classification")]
    public required string ProductClassification { get; init; }

    [BsonElement("models")]
    public List<Model> Models { get; set; } = [];

    [BsonElement("colors")]
    public List<Color> Colors { get; set; } = [];

    [BsonElement("storages")]
    public List<Storage> Storages { get; set; } = [];

    [BsonElement("prices")]
    public List<SkuPriceList> Prices { get; set; } = [];

    [BsonElement("showcase_image")]
    public List<Image> ShowcaseImages { get; set; } = [];

    [BsonElement("description")]
    public string Description { get; set; } = default!;

    [BsonElement("overall_sold")]
    public int OverallSold { get; set; } = 0;

    [BsonElement("promotion")]
    public Promotion? Promotion { get; set; }

    [BsonElement("is_newest")]
    public bool IsNewest { get; set; } = false;

    [BsonElement("slug")]
    public required Slug Slug { get; init; }

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedBy")]
    public string? UpdatedBy { get; set; } = null;

    [BsonElement("IsDeleted")]
    public bool IsDeleted { get; set; } = false;

    [BsonElement("DeletedAt")]
    public DateTime? DeletedAt { get; set; } = null;

    [BsonElement("DeletedBy")]
    public string? DeletedBy { get; set; } = null;


    public static ProductModel Create(ModelId productModelId,
                                      Category category,
                                      string name,
                                      EProductClassification productClassification,
                                      List<Model> models,
                                      List<Color> colors,
                                      List<Storage> storages,
                                      List<SkuPriceList> prices,
                                      List<Image> showcaseImages,
                                      string description,
                                      Promotion? promotion,
                                      bool isNewest = false,
                                      int? overallSold = 0)
    {
        var newModel = new ProductModel(productModelId)
        {
            Category = category,
            Name = name,
            NormalizedModel = SnakeCaseSerializer.Serialize(name).ToUpper(),
            ProductClassification = productClassification.Name,
            Models = models,
            Colors = colors,
            Storages = storages,
            Prices = prices,
            ShowcaseImages = showcaseImages,
            Description = description,
            OverallSold = (int)overallSold!,
            Promotion = promotion,
            IsNewest = isNewest,
            Slug = Slug.Create(name)
        };

        return newModel;
    }

    public ProductModelResponse ToResponse()
    {
        return new ProductModelResponse
        {
            Id = Id.Value!,
            Category = Category.ToResponse(),
            Name = Name,
            NormalizedModel = NormalizedModel,
            ProductClassification = ProductClassification,
            ModelItems = Models.Select(m => m.ToResponse()).ToList(),
            ColorItems = Colors.Select(c => c.ToResponse()).ToList(),
            StorageItems = Storages.Select(s => s.ToResponse()).ToList(),
            SkuPrices = Prices.Select(p => p.ToResponse()).ToList(),
            Description = Description,
            ShowcaseImages = ShowcaseImages.Select(img => img.ToResponse()).ToList(),
            Promotion = Promotion?.ToResponse(),
            IsNewest = IsNewest,
            Slug = Slug.Value!,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
