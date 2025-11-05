

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.Iphone.Queries.GetIphoneModels;

public sealed record GetIphoneModelsQuery() : IQuery<PaginationResponse<IphoneModelResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public List<string>? Colors { get; init; } = new List<string>();
    public List<string>? Storages { get; init; } = new List<string>();
    public List<string>? Models { get; init; } = new List<string>();
    public string? MinPrice { get; init; }
    public string? MaxPrice { get; init; }
    public string? PriceSort { get; init; }
}
