

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.IPhone.Queries.GetIphoneModels;

public sealed record GetIphoneModelsQuery() : IQuery<PaginationResponse<IphoneModelResponse>>
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public List<string>? Colors { get; set; } = new List<string>();
    public List<string>? Storages { get; set; } = new List<string>();
    public List<string>? Models { get; set; } = new List<string>();
    public string? MinPrice { get; set; }
    public string? MaxPrice { get; set; }
    public string? PriceSort { get; set; }
}
