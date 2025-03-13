

using DnsClient;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class AverageRating : ValueObject
{
    [BsonElement("average_value")]
    public double AverageValue { get; set; }

    [BsonElement("average_num_ratings")]
    public int NumRatings { get; set; }

    private AverageRating(double averageValue, int numRatings)
    {
        AverageValue = averageValue;
        NumRatings = numRatings;
    }

    public static AverageRating CreateNew()
    {
        return new AverageRating(0, 0);
    }

    public void AddNewRating(Rating rating)
    {
        AverageValue = (AverageValue * NumRatings + rating.Value) / ++NumRatings;
    }

    public void RemoveRating(Rating rating)
    {
        AverageValue = (AverageValue * NumRatings - rating.Value) / --NumRatings;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return AverageValue;
        yield return NumRatings;
    }
}
