

using MongoDB.Bson.Serialization.Attributes;
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
        RatingCount += 1;
        RatingAverageValue = (RatingAverageValue * RatingCount + rating) / RatingCount;
    }

    public void UpdateRating(Rating oldRating, Rating newRating)
    {
        RatingAverageValue = (RatingAverageValue * RatingCount - oldRating.Value + newRating.Value) / RatingCount;
    }

    public void RemoveRating(Rating rating)
    {
        RatingCount -= 1;
        RatingAverageValue = (RatingAverageValue * RatingCount - rating.Value) / RatingCount;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return RatingAverageValue;
        yield return RatingCount;
    }
}
