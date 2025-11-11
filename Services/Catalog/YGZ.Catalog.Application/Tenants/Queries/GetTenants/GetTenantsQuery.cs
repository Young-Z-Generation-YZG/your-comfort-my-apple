using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenants;

public sealed record GetTenantsQuery() : IQuery<List<TenantResponse>>
{
    // public int? Page { get; init; }
    // public int? Limit { get; init; }
    // public string? TenantName { get; init; }
    // public string? TenantType { get; init; }
}
