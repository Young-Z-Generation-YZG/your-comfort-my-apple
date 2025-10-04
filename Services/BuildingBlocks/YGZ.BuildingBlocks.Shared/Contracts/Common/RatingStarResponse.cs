

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record RatingStarResponse
{
    required public int Star { get; set; }
    required public int Count { get; set; }
}