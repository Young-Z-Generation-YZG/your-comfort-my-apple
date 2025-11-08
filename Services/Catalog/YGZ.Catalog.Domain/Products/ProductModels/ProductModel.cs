using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;

namespace YGZ.Catalog.Domain.Products.ProductModels;

[BsonCollection("ProductModels")]
public class ProductModel : AggregateRoot<ModelId>, IAuditable, ISoftDelete
{
    public ProductModel(ModelId id) : base(id)
    {
        RatingStars = InitRatingStar();
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

    [BsonElement("average_rating")]
    public AverageRating AverageRating { get; set; } = AverageRating.Create(0, 0);

    [BsonElement("rating_stars")]
    public List<RatingStar> RatingStars { get; set; }

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
                                         AverageRating averageRating,
                                     List<RatingStar> ratingStars,
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
            AverageRating = averageRating,
            RatingStars = ratingStars,
            OverallSold = (int)overallSold!,
            Promotion = promotion,
            IsNewest = isNewest,
            Slug = Slug.Create(name)
        };

        return newModel;
    }

    protected List<RatingStar> InitRatingStar()
    {
        var ratingStars = new List<RatingStar>();

        for (int i = 1; i <= 5; i++)
        {
            ratingStars.Add(RatingStar.Create(i, 0));
        }

        return ratingStars;
    }

    public void AddNewRating(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 5");

        AverageRating.AddNewRating(review.Rating);

        RatingStars.FirstOrDefault(x => x.Star == review.Rating)!.Count += 1;
    }

    public void UpdateRating(Review oldReview, Review newReview)
    {
        if (oldReview.Rating < 1 || oldReview.Rating > 5)
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 5");

        if (newReview.Rating < 1 || newReview.Rating > 5)
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 5");

        AverageRating.UpdateRating(oldReview.Rating, newReview.Rating);

        RatingStars.FirstOrDefault(x => x.Star == oldReview.Rating)!.Count -= 1;
        RatingStars.FirstOrDefault(x => x.Star == newReview.Rating)!.Count += 1;
    }

    public void DeleteRating(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentOutOfRangeException("Rating must be between 1 and 5");

        AverageRating.RemoveRating(review.Rating);

        RatingStars.FirstOrDefault(x => x.Star == review.Rating)!.Count -= 1;
    }

    public void SetIsNewest(bool isNewest)
    {
        IsNewest = isNewest;
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
            ShowcaseImages = ShowcaseImages.Select(img => img.ToResponse()).ToList(),
            Description = Description,
            AverageRating = AverageRating.ToResponse(),
            RatingStars = RatingStars.Select(x => x.ToResponse()).ToList(),
            OverallSold = OverallSold,
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
