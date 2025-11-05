using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenants;

public sealed record GetTenantsQuery() : IQuery<PaginationResponse<TenantResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? TenantName { get; init; }
    public string? TenantType { get; init; }
}
