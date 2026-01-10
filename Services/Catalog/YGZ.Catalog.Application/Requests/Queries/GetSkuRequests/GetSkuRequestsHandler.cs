using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Requests.SkuRequest;
using YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;

namespace YGZ.Catalog.Application.Requests.Queries.GetSkuRequests;

internal sealed class GetSkuRequestsHandler : IQueryHandler<GetSkuRequestsQuery, PaginationResponse<SkuRequestResponse>>
{
    private readonly ILogger<GetSkuRequestsHandler> _logger;
    private readonly IMongoRepository<SkuRequest, RequestId> _skuRequestRepository;

    public GetSkuRequestsHandler(
        ILogger<GetSkuRequestsHandler> logger,
        IMongoRepository<SkuRequest, RequestId> skuRequestRepository)
    {
        _logger = logger;
        _skuRequestRepository = skuRequestRepository;
    }

    public async Task<Result<PaginationResponse<SkuRequestResponse>>> Handle(GetSkuRequestsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[QueryHandler:{QueryHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(GetSkuRequestsHandler), "Getting paginated SKU requests", request);

        var (filter, sort) = GetFilterDefinition(request);

        var result = await _skuRequestRepository.GetAllAsync(
            _page: request.Page,
            _limit: request.Limit,
            filter: filter,
            sort: sort,
            cancellationToken: cancellationToken);

        var response = MapToResponse(result, request);

        _logger.LogInformation(":::[QueryHandler:{QueryHandler}]::: Successfully retrieved {Count} SKU requests. Total: {Total}",
            nameof(GetSkuRequestsHandler), response.Items.Count(), result.totalRecords);

        return response;
    }

    private PaginationResponse<SkuRequestResponse> MapToResponse((List<SkuRequest> items, int totalRecords, int totalPages) result, GetSkuRequestsQuery request)
    {
        var items = result.items.Select(sr => sr.ToResponse()).ToList();

        var links = new PaginationLinks("", "", "", "");

        return new PaginationResponse<SkuRequestResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Links = links,
            Items = items
        };
    }

    private (MongoDB.Driver.FilterDefinition<SkuRequest>? filter, MongoDB.Driver.SortDefinition<SkuRequest>? sort) GetFilterDefinition(GetSkuRequestsQuery request)
    {
        var filterBuilder = Builders<SkuRequest>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrWhiteSpace(request.RequestState))
        {
            filter &= filterBuilder.Eq(x => x.State, request.RequestState);
        }

        if (!string.IsNullOrWhiteSpace(request.BranchId) && ObjectId.TryParse(request.BranchId, out var branchObjectId))
        {
            if (!string.IsNullOrWhiteSpace(request.TransferType))
            {
                if (request.TransferType.ToUpper() == "SENT_TO")
                {
                    filter &= filterBuilder.Eq("from_branch.branch_id", branchObjectId);
                }
                else if (request.TransferType.ToUpper() == "RECEIVE_FROM")
                {
                    filter &= filterBuilder.Eq("to_branch.branch_id", branchObjectId);
                }
            }
            else
            {
                filter &= filterBuilder.Or(
                    filterBuilder.Eq("to_branch.branch_id", branchObjectId),
                    filterBuilder.Eq("from_branch.branch_id", branchObjectId)
                );
            }
        }

        var sortBuilder = Builders<SkuRequest>.Sort;
        var sort = sortBuilder.Descending(x => x.CreatedAt);

        return (filter, sort);
    }
}
