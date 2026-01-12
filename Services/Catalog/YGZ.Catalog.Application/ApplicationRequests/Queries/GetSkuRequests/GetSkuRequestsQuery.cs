using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.Requests.Queries.GetSkuRequests;

public sealed record GetSkuRequestsQuery : IQuery<PaginationResponse<SkuRequestResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? RequestState { get; init; }
    public string? TransferType { get; init; }
    public string? BranchId { get; init; }
}
