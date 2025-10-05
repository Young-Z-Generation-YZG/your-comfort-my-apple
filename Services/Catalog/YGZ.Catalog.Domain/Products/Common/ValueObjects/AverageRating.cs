

using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class AverageRating : ValueObject
{
    [BsonElement("rating_average_value")]
    public decimal RatingAverageValue { get; set; }

    [BsonElement("rating_count")]
    public int RatingCount { get; set; }

    private AverageRating(decimal ratingAverageValue, int ratingCount)
    {
        RatingAverageValue = ratingAverageValue;
        RatingCount = ratingCount;
    }

    public static AverageRating Create(decimal averageValue = 0, int averageNumRating = 0)
    {
        return new AverageRating(averageValue, averageNumRating);
    }

    public void AddNewRating(int rating)
    {
        RatingAverageValue = (RatingAverageValue * RatingCount + rating) / ++RatingCount;
    }

    public void UpdateRating(int oldRating, int newRating)
    {
        RatingAverageValue = (RatingAverageValue * RatingCount - oldRating + newRating) / RatingCount;
    }

    public void RemoveRating(int rating)
    {
        RatingAverageValue = (RatingAverageValue * RatingCount - rating) / RatingCount--;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return RatingAverageValue;
        yield return RatingCount;
    }

    public AverageRatingResponse ToResponse()
    {
        return new AverageRatingResponse
        {
            RatingAverageValue = RatingAverageValue,
            RatingCount = RatingCount
        };
    }
}
