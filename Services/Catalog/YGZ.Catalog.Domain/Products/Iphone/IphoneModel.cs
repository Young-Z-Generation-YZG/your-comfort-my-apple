using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Products.ProductModels.Events;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Iphone;

[BsonCollection("IphoneModels")]
public class IphoneModel : AggregateRoot<ModelId>, IAuditable, ISoftDelete
{
    public IphoneModel(ModelId id) : base(id)
    {
        RatingStars = InitRatingStar();
    }

    [BsonElement("category")]
    public required Category Category { get; init; }

    [BsonElement("name")]
    public string Name { get; set; } = default!;

    [BsonElement("normalized_model")]
    public string NormalizedModel { get; init; } = default!;

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

    [BsonElement("average_rating")]
    public AverageRating AverageRating { get; set; } = AverageRating.Create(0, 0);

    [BsonElement("rating_stars")]
    public List<RatingStar> RatingStars { get; set; }

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

    protected List<RatingStar> InitRatingStar()
    {
        var ratingStars = new List<RatingStar>();

        for (int i = 1; i <= 5; i++)
        {
            ratingStars.Add(RatingStar.Create(i, 0));
        }

        return ratingStars;
    }

    public static IphoneModel Create(ModelId iPhoneModelId,
                                     Category category,
                                     string name,
                                     List<Model> models,
                                     List<Color> colors,
                                     List<Storage> storages,
                                     List<SkuPriceList> prices,
                                     List<Image> showcaseImages,
                                     string description,
                                     AverageRating averageRating,
                                     List<RatingStar> ratingStars,
                                     bool isNewest,
                                     int? overallSold = 0)
    {
        var newModel = new IphoneModel(iPhoneModelId)
        {
            Category = category,
            Name = name,
            NormalizedModel = SnakeCaseSerializer.Serialize(name).ToUpper(),
            ProductClassification = EProductClassification.IPHONE.Name,
            Models = models,
            Colors = colors,
            Storages = storages,
            Prices = prices,
            ShowcaseImages = showcaseImages,
            Description = description,
            AverageRating = averageRating,
            OverallSold = (int)overallSold!,
            IsNewest = isNewest,
            Slug = Slug.Create(name),
        };

        List<SkuPriceList> productModelPrices = newModel.Prices.Select(p => SkuPriceList.Create(p.SkuId, p.NormalizedModel, p.NormalizedColor, p.NormalizedStorage, p.UnitPrice)).ToList();

        var productModel = ProductModel.Create(productModelId: newModel.Id,
                                               category: newModel.Category,
                                               name: newModel.Name,
                                               productClassification: EProductClassification.IPHONE,
                                               models: newModel.Models,
                                               colors: newModel.Colors,
                                               storages: newModel.Storages,
                                               prices: productModelPrices,
                                               showcaseImages: newModel.ShowcaseImages,
                                               description: newModel.Description,
                                               averageRating: newModel.AverageRating,
                                               ratingStars: newModel.RatingStars,
                                               promotion: null,
                                               isNewest: newModel.IsNewest);

        newModel.AddDomainEvent(new IphoneModelCreatedDomainEvent(newModel, productModel));

        return newModel;
    }

    public void SetIsNewest(bool isNewest)
    {
        IsNewest = isNewest;
    }
}
