
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record IPhoneModelResponse
{
    required public string ModelId { get; set; }
    required public string ModelName { get; set; }
    required public List<ModelItemResponse> ModelItems { get; set; }
    required public List<ColorResponse> ColorItems { get; set; }
    required public List<StorageResponse> StorageItems { get; set; }
    required public string GeneralModel { get; set; }
    required public string ModelDescription { get; set; }
    required public int OverallSold { get; set; }
    required public AverageRatingResponse AverageRating { get; set; }
    required public List<RatingStarResponse> RatingStars { get; set; }
    required public List<ImageResponse> ModelImages { get; set; }
    required public string ModelSlug { get; set; }
    required public string CategoryId { get; set; }
    public bool? IsDeleted { get; set; } = null;
    public string? DeletedBy { get; set; } = null;
    public DateTime? CreatedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? DeletedAt { get; set; } = null;
}