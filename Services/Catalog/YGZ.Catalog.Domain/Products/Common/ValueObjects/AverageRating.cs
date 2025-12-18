

using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class AverageRating : ValueObject
{
    [BsonElement("rating_average_value")]
    private decimal _ratingAverageValue;

    [BsonElement("rating_count")]
    public int RatingCount { get; set; }

    [BsonIgnore]
    public decimal RatingAverageValue
    {
        get => Math.Round(_ratingAverageValue, 1, MidpointRounding.AwayFromZero);
        set => _ratingAverageValue = value;
    }

    private AverageRating(decimal ratingAverageValue, int ratingCount)
    {
        _ratingAverageValue = ratingAverageValue;
        RatingCount = ratingCount;
    }

    public static AverageRating Create(decimal averageValue = 0, int averageNumRating = 0)
    {
        return new AverageRating(averageValue, averageNumRating);
    }

    public void AddNewRating(int rating)
    {
        _ratingAverageValue = (_ratingAverageValue * RatingCount + rating) / ++RatingCount;
    }

    public void UpdateRating(int oldRating, int newRating)
    {
        if (RatingCount > 0)
        {
            _ratingAverageValue = (_ratingAverageValue * RatingCount - oldRating + newRating) / RatingCount;
        }
    }

    public void RemoveRating(int rating)
    {
        if (RatingCount > 0)
        {
            RatingCount--;
            if (RatingCount > 0)
            {
                _ratingAverageValue = (_ratingAverageValue * (RatingCount + 1) - rating) / RatingCount;
            }
            else
            {
                _ratingAverageValue = 0;
            }
        }
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
