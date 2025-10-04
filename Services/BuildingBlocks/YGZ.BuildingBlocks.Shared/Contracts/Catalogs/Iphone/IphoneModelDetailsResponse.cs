using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record IphoneModelDetailsResponse
{
    required public string Id { get; init; }
    required public string CategoryId { get; init; }
    required public string Name { get; init; }
    required public List<ModelResponse> ModelItems { get; init; }
    required public List<ColorResponse> ColorItems { get; init; }
    required public List<StorageResponse> StorageItems { get; init; }
    public string? Description { get; init; }
    required public int OverallSold { get; init; }
    required public AverageRatingResponse AverageRating { get; init; }
    required public List<BranchWithSkusResponse> Branchs { get; init; }
    // required public List<RatingStarResponse> RatingStars { get; set; }
    // required public List<ImageResponse> ModelImages { get; set; }
}
