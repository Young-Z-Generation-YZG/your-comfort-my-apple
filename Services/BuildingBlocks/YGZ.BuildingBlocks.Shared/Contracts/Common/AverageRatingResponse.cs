

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record AverageRatingResponse
{
    required public decimal RatingAverageValue { get; set; }
    required public int RatingCount { get; set; }
}
