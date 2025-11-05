
using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

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

    [BsonElement("models")]
    public List<Model> Models { get; set; } = [];

    [BsonElement("colors")]
    public List<Color> Colors { get; set; } = [];

    [BsonElement("storages")]
    public List<Storage> Storages { get; set; } = [];

    [BsonElement("prices")]
    public List<IphoneSkuPriceList> Prices { get; set; } = [];

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
                                     List<IphoneSkuPriceList> prices,
                                     List<Image> showcaseImages,
                                     string description,
                                     AverageRating averageRating,
                                     List<RatingStar> ratingStars,
                                     int? overallSold = 0)
    {
        var newModel = new IphoneModel(iPhoneModelId)
        {
            Category = category,
            Name = name,
            NormalizedModel = SnakeCaseSerializer.Serialize(name).ToUpper(),
            Models = models,
            Colors = colors,
            Storages = storages,
            Prices = prices,
            ShowcaseImages = showcaseImages,
            Description = description,
            AverageRating = averageRating,
            OverallSold = (int)overallSold!,
            Slug = Slug.Create(name),
        };

        return newModel;
    }

    public decimal? GetPrice(Model model, Color color, Storage storage)
    {
        return Prices.FirstOrDefault(p =>
            p.NormalizedModel == model.NormalizedName &&
            p.NormalizedColor == color.NormalizedName &&
            p.NormalizedStorage == storage.NormalizedName)?.UnitPrice;
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
}
