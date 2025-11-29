using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenantById;

public sealed record GetTenantByIdQuery : IQuery<TenantResponse> {
    public required string TenantId { get; init; }
}