

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Common.ValueObjects;

public class StarRating : ValueObject
{
    [BsonElement("star")]
    public int Star { get; set; }

    [BsonElement("num_ratings")]
    public int NumRatings { get; set; }

    public StarRating(int star, int numRatings)
    {
        Star = star;
        NumRatings = numRatings;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<StarRating> CreateNewList(int length)
    {
        for (int i = 1; i <= length; i++)
        {
            yield return new StarRating(i, 0);
        }
    }
}
