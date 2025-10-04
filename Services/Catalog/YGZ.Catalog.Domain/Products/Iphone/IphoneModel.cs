
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.Events;

namespace YGZ.Catalog.Domain.Products.Iphone;

[BsonCollection("IphoneModels")]
public class IphoneModel : AggregateRoot<ModelId>, IAuditable, ISoftDelete
{
    public IphoneModel(ModelId id) : base(id)
    {
        RatingStars = InitRatingStar();
    }

    [BsonElement("category_id")]
    public required CategoryId CategoryId { get; init; }

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

    [BsonElement("showcase_image")]
    public List<Image> ShowcaseImage { get; set; } = [];

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

    [BsonElement("created_at")]
    public DateTime CreatedAt => DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => DateTime.Now;

    [BsonElement("modified_by")]
    public string? ModifiedBy => null;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by")]
    public string? DeletedBy => null;

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
                                     string name,
                                     List<Model> models,
                                     List<Color> colors,
                                     List<Storage> storages,
                                     List<Image> showcaseImages,
                                     string description,
                                     AverageRating averageRating,
                                     List<RatingStar> ratingStars,
                                     CategoryId categoryId,
                                     int? overallSold = 0)
    {
        var newModel = new IphoneModel(iPhoneModelId)
        {
            Name = name,
            NormalizedModel = NormalizeString.Normalize(name),
            Models = models,
            Colors = colors,
            Storages = storages,
            ShowcaseImage = showcaseImages,
            Description = description,
            AverageRating = averageRating,
            OverallSold = (int)overallSold!,
            Slug = Slug.Create(name),
            CategoryId = categoryId,
        };

        return newModel;
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
