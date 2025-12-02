using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.Inventory.Queries.GetWarehouses;

public sealed record GetSkusQuery : IQuery<PaginationResponse<SkuWithImageResponse>>
{
    public string? _tenantId { get; set; }
    public int? _page { get; init; }
    public int? _limit { get; init; }
    public List<string>? _colors { get; init; }
    public List<string>? _storages { get; init; }
    public List<string>? _models { get; init; }
    
    // Dynamic filters for stock
    public int? _stock { get; init; }
    public string? _stockOperator { get; init; } // ">=", ">", "<", "<=", "==", "!=", "in"
    
    // Dynamic filters for sold
    public int? _sold { get; init; }
    public string? _soldOperator { get; init; } // ">=", ">", "<", "<=", "==", "!=", "in"
}
