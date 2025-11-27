using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenantById;

public class GetTenantByIdHandler : IQueryHandler<GetTenantByIdQuery, TenantResponse>
{
    private readonly ILogger<GetTenantByIdHandler> _logger;
    private readonly IMongoRepository<Tenant, TenantId> _repository;

    public GetTenantByIdHandler(IMongoRepository<Tenant, TenantId> repository,
                                ILogger<GetTenantByIdHandler> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<TenantResponse>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.TenantId, ignoreBaseFilter: true, cancellationToken: cancellationToken);

        if (tenant is null)
        {
            return Errors.Tenant.DoesNotExist;
        }

        return tenant.ToResponse();
    }
}