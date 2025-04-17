
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record IPhoneDetailsWithPromotionResponse
{
    required public string ProductId { get; set; }
    required public string ProductModel { get; set; }
    required public ColorResponse ProductColor { get; set; }
    required public StorageResponse ProductStorage { get; set; }
    required public decimal ProductUnitPrice { get; set; }
    required public int ProductAvailableInStock { get; set; }
    required public int TotalSold { get; set; }
    required public string ProductState { get; set; }
    required public string ProductDescription { get; set; }
    required public string ProductNameTag { get; set; }
    required public List<ImageResponse> ProductImages { get; set; }
    public IPhonePromotionResponse? Promotion { get; set; } = null;
    required public string ProductSlug { get; set; }
    required public string IphoneModelId { get; set; }
    required public string CategoryId { get; set; }
    public bool? IsDeleted { get; set; } = null;
    public string? DeletedBy { get; set; } = null;
    public DateTime? CreatedAt { get; set; } = null;
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? DeletedAt { get; set; } = null;
}
