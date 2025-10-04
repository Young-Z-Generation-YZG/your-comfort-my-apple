
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record IphoneModelWithPromotionResponse()
{
    public string ModelId { get; set; } = default;
    public string ModelName { get; set; } = default;
    public List<ModelResponse> ModelItems { get; set; } = default;
    public List<ColorResponse> ColorItems { get; set; } = default;
    public List<StorageResponse> StorageItems { get; set; } = default;
    public string GeneralModel { get; set; } = default;
    public string ModelDescription { get; set; } = default;
    public decimal MinimunUnitPrice { get; set; } = default;
    public decimal MaximunUnitPrice { get; set; } = default;
    public int OverallSold { get; set; } = default;
    public AverageRatingResponse AverageRating { get; set; } = default;
    public List<RatingStarResponse> RatingStars { get; set; } = default;
    public List<ImageResponse> ModelImages { get; set; } = default;
    public ModelPromotionResponse? ModelPromotion { get; set; } = null;
    public string ModelSlug { get; set; } = default;
    public string CategoryId { get; set; } = default;
    public bool? IsDeleted { get; set; } = null;
    public string? DeletedBy { get; set; } = null;
    public DateTime? CreatedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? DeletedAt { get; set; } = null;
}
