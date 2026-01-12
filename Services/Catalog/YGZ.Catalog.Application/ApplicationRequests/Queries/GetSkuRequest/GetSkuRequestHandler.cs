using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Requests.SkuRequest;
using YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;

namespace YGZ.Catalog.Application.Requests.Queries.GetSkuRequest;

internal sealed class GetSkuRequestHandler : IQueryHandler<GetSkuRequestQuery, SkuRequestResponse>
{
    private readonly ILogger<GetSkuRequestHandler> _logger;
    private readonly IMongoRepository<SkuRequest, RequestId> _skuRequestRepository;

    public GetSkuRequestHandler(
        ILogger<GetSkuRequestHandler> logger,
        IMongoRepository<SkuRequest, RequestId> skuRequestRepository)
    {
        _logger = logger;
        _skuRequestRepository = skuRequestRepository;
    }

    public async Task<Result<SkuRequestResponse>> Handle(GetSkuRequestQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[QueryHandler:{QueryHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(GetSkuRequestHandler), "Getting SKU request by ID", request);

        var skuRequest = await _skuRequestRepository.GetByIdAsync(request.Id, cancellationToken);

        if (skuRequest is null)
        {
            _logger.LogWarning(":::[QueryHandler:{QueryHandler}][Result:Error]::: SKU request not found. ID: {Id}",
                nameof(GetSkuRequestHandler), request.Id);

            return Errors.SkuRequest.NotFound;
        }

        _logger.LogInformation(":::[QueryHandler:{QueryHandler}]::: Successfully retrieved SKU request. ID: {Id}",
            nameof(GetSkuRequestHandler), request.Id);

        return skuRequest.ToResponse();
    }
}
