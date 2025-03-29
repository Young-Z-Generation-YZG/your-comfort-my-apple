
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record IPhoneResponse
{
    public string? ProductId { get; set; }
    public string? ProductModel { get; set; }
    public ColorResponse? ProductColor { get; set; }
    public object? ProductStorage { get; set; }
    public decimal? ProductUnitPrice { get; set; }
    public int? ProductAvailableInStock { get; set; }
    public string? ProductDescription { get; set; }
    public List<ImageResponse>? ProductImages { get; set; }
    public string? ProductSlug { get; set; }
}
