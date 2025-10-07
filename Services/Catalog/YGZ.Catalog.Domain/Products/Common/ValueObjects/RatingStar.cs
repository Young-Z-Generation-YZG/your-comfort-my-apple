using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class RatingStar : ValueObject
{
    required public int Star { get; set; }
    public int Count { get; set; } = 0;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Star;
        yield return Count;
    }

    public static RatingStar Create(int star, int count)
    {
        return new RatingStar { Star = star, Count = count };
    }

    public RatingStarResponse ToResponse()
    {
        return new RatingStarResponse
        {
            Star = Star,
            Count = Count
        };
    }
}
