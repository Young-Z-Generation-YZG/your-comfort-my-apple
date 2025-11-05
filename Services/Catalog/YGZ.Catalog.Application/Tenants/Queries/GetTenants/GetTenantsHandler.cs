using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Tenants;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Tenants;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Tenants.Queries.GetTenants;

public class GetTenantsHandler : IQueryHandler<GetTenantsQuery, PaginationResponse<TenantResponse>>
{
    private readonly ILogger<GetTenantsHandler> _logger;
    private readonly IMongoRepository<Tenant, TenantId> _repository;

    public GetTenantsHandler(ILogger<GetTenantsHandler> logger, IMongoRepository<Tenant, TenantId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<PaginationResponse<TenantResponse>>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        var filter = BuildFilter(request);

        var result = await _repository.GetAllAsync(
            _page: request.Page,
            _limit: request.Limit,
            filter: filter,
            sort: null,
            cancellationToken: cancellationToken);

        var response = MapToResponse(result, request);

        return response;
    }

    private static FilterDefinition<Tenant> BuildFilter(GetTenantsQuery request)
    {
        var filterBuilder = Builders<Tenant>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrWhiteSpace(request.TenantName))
        {
            filter &= filterBuilder.Regex("name", new MongoDB.Bson.BsonRegularExpression(request.TenantName, "i"));
        }

        if (!string.IsNullOrWhiteSpace(request.TenantType))
        {
            filter &= filterBuilder.Eq("tenant_type", request.TenantType);
        }

        return filter;
    }

    private static PaginationResponse<TenantResponse> MapToResponse((List<Tenant> items, int totalRecords, int totalPages) result, GetTenantsQuery request)
    {
        var items = result.items.Select(tenant => tenant.ToResponse()).ToList();

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            request: request,
            currentPage: request.Page,
            totalPages: result.totalPages);

        return new PaginationResponse<TenantResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Links = links,
            Items = items
        };
    }
}
