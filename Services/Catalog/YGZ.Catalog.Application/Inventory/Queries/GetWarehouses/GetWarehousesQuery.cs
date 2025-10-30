using System.Diagnostics.CodeAnalysis;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.Inventory.Queries.GetWarehouses;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Underscore prefix is used for query parameters matching API conventions")]
public sealed record GetWarehousesQuery : IQuery<PaginationResponse<SkuWithImageResponse>>
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public List<string>? _colors { get; init; } = new List<string>();
    public List<string>? _storages { get; init; } = new List<string>();
    public List<string>? _models { get; init; } = new List<string>();
}
