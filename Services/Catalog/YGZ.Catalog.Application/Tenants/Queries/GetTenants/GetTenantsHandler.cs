using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenants;

public class GetTenantsHandler : IQueryHandler<GetTenantsQuery, List<TenantResponse>>
{
    private readonly ILogger<GetTenantsHandler> _logger;
    private readonly IMongoRepository<Tenant, TenantId> _repository;

    public GetTenantsHandler(ILogger<GetTenantsHandler> logger, IMongoRepository<Tenant, TenantId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<TenantResponse>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        var response = MapToResponse(result);

        return response;
    }

    private static List<TenantResponse> MapToResponse(List<Tenant> tenants)
    {
        return tenants.Select(tenant => tenant.ToResponse()).ToList();
    }
}
