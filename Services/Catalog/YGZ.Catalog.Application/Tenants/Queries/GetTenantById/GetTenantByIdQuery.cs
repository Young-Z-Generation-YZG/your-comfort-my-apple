using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenantById;

public sealed record GetTenantByIdQuery(string TenantId) : IQuery<TenantResponse>;