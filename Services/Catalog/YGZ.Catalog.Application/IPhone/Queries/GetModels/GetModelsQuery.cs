

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.WithPromotion;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.IPhone.Queries.GetModels;

public sealed record GetModelsQuery() : IQuery<PaginationResponse<IphoneModelWithPromotionResponse>> 
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public List<string>? ProductColors { get; set; } = new List<string>();
    public List<string>? ProductStorages { get; set; } = new List<string>();
    public List<string>? ProductModels { get; set; } = new List<string>();
    public string? PriceFrom { get; set; }
    public string? PriceTo { get; set; }
    public string? PriceSort { get; set; }
}
