

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Iphone16;

[BsonCollection("IphoneModels")]
public class IPhone16Model : AggregateRoot<IPhone16ModelId>, IAuditable, ISoftDelete
{
    public IPhone16Model(IPhone16ModelId id) : base(id)
    {
        RatingStars = InitRatingStar();
    }

    private IPhone16Model() : base(null!)
    {
        RatingStars = InitRatingStar();
    }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("models")]
    public List<ModelBK> Models { get; set; }

    [BsonElement("colors")]
    public List<ColorBK> Colors { get; set; } = [];

    [BsonElement("storages")]
    public List<Storage> Storages { get; set; } = [];

    [BsonElement("general_model")]
    public string GeneralModel { get; set; } = default!;

    [BsonElement("description")]
    public string Description { get; set; } = default!;

    [BsonElement("overall_sold")]
    public int OverallSold { get; set; } = 0;

    [BsonElement("average_rating")]
    public AverageRating AverageRating { get; set; } = AverageRating.Create(0, 0);

    [BsonElement("rating_stars")]
    public List<RatingStar> RatingStars { get; set; }

    [BsonElement("description_images")]
    public List<Image> DescriptionImages { get; set; } = [];

    [BsonElement("slug")]
    public Slug Slug { get; set; }

    [BsonElement("category_id")]
    public CategoryId? CategoryId { get; set; } = null;

    [BsonElement("created_at")]
    public DateTime CreatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id.Id?.CreationTime ?? DateTime.Now;

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

    public static IPhone16Model Create(IPhone16ModelId iPhone16ModelId,
                                       string name,
                                       List<ModelBK> models,
                                       List<ColorBK> colors,
                                       List<Storage> storages,
                                       string description,
                                       List<Image> descriptionImages,
                                       AverageRating averageRating,
                                       List<RatingStar> ratingStars,
                                       CategoryId? categoryId,
                                       int? overallSold = 0)
    {
        return new IPhone16Model(iPhone16ModelId)
        {
            Name = name,
            Models = models,
            Colors = colors,
            Storages = storages,
            GeneralModel = Slug.Create(name).Value,
            Description = description,
            AverageRating = averageRating,
            OverallSold = (int)overallSold!,
            DescriptionImages = descriptionImages,
            Slug = Slug.Create(name),
            CategoryId = categoryId,
        };
    }

    public void AddNewRating(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentOutOfRangeException(nameof(review.Rating), "Rating must be between 1 and 5.");

        AverageRating.AddNewRating(review.Rating);

        RatingStars.FirstOrDefault(x => x.Star == review.Rating)!.Count += 1;
    }

    public void UpdateRating(Review oldReview, Review newReview)
    {
        if (oldReview.Rating < 1 || oldReview.Rating > 5)
            throw new ArgumentOutOfRangeException(nameof(oldReview.Rating), "Rating must be between 1 and 5.");

        if (newReview.Rating < 1 || newReview.Rating > 5)
            throw new ArgumentOutOfRangeException(nameof(newReview.Rating), "Rating must be between 1 and 5.");

        AverageRating.UpdateRating(oldReview.Rating, newReview.Rating);

        RatingStars.FirstOrDefault(x => x.Star == oldReview.Rating)!.Count -= 1;
        RatingStars.FirstOrDefault(x => x.Star == newReview.Rating)!.Count += 1;
    }

    public void DeleteRating(Review review)
    {
        if (review.Rating < 1 || review.Rating > 5)
            throw new ArgumentOutOfRangeException(nameof(review.Rating), "Rating must be between 1 and 5.");

        AverageRating.RemoveRating(review.Rating);

        RatingStars.FirstOrDefault(x => x.Star == review.Rating)!.Count -= 1;
    }
}
