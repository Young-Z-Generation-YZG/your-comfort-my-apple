

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
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
    public List<Model> Models { get; set; }

    [BsonElement("colors")]
    public List<Color> Colors { get; set; } = [];

    [BsonElement("storages")]
    public List<Storage> Storages { get; set; } = [];

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

    [BsonElement("created_at")]
    public DateTime CreatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    [BsonElement("modified_by")]
    public Guid? ModifiedBy => null;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by")]
    public Guid? DeletedBy => null;

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
                                       List<Model> models,
                                       List<Color> colors,
                                       List<Storage> storages,
                                       List<Image> descriptionImages,
                                       AverageRating averageRating,
                                       List<RatingStar> ratingStars,
                                       string description,
                                       int? overallSold = 0)
    {
        return new IPhone16Model(iPhone16ModelId)
        {
            Name = name,
            Models = models,
            Colors = colors,
            Storages = storages,
            Description = description,
            AverageRating = averageRating,
            OverallSold = (int)overallSold!,
            DescriptionImages = descriptionImages,
            Slug = Slug.Create(name),
        };
    }
}
