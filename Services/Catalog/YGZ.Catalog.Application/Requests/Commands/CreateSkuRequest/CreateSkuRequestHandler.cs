using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Requests.SkuRequest;
using YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Requests.Commands.CreateSkuRequest;

public class CreateSkuRequestHandler : ICommandHandler<CreateSkuRequestCommand, bool>
{
    private readonly ILogger<CreateSkuRequestHandler> _logger;
    private readonly IMongoRepository<SkuRequest, RequestId> _skuRequestRepository;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;
    private readonly IDistributedCache _distributedCache;

    public CreateSkuRequestHandler(IMongoRepository<SkuRequest, RequestId> skuRequestRepository,
                                   IMongoRepository<SKU, SkuId> skuRepository,
                                   ILogger<CreateSkuRequestHandler> logger,
                                   IMongoRepository<Branch, BranchId> branchRepository,
                                   IDistributedCache distributedCache)
    {
        _skuRequestRepository = skuRequestRepository;
        _skuRepository = skuRepository;
        _branchRepository = branchRepository;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateSkuRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Processing create SKU request command. Parameters: {@Parameters}",
            nameof(CreateSkuRequestHandler), request);

        var sku = await _skuRepository.GetByIdAsync(request.SkuId, cancellationToken);

        if (sku is null)
        {
            _logger.LogWarning(":::[CommandHandler:{CommandHandler}][Result:Error]::: SKU not found. Parameters: {@Parameters}",
                nameof(CreateSkuRequestHandler), new { request.SkuId });
            return Errors.SkuRequest.SkuNotFound;
        }

        var fromBranch = await _branchRepository.GetByIdAsync(request.FromBranchId, cancellationToken);
        var toBranch = await _branchRepository.GetByIdAsync(request.ToBranchId, cancellationToken);

        if (fromBranch is null || toBranch is null)
        {
            _logger.LogWarning(":::[CommandHandler:{CommandHandler}][Result:Error]::: Branch not found. Parameters: {@Parameters}",
                nameof(CreateSkuRequestHandler), new { request.FromBranchId, request.ToBranchId });
            return Errors.Brancch.DoesNotExist;
        }

        EColor.TryFromName(sku.Color.NormalizedName, out var colorEnum);

        var imageUrl = string.Empty;
        if (colorEnum is not null)
        {
            var imageCacheKey = CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(modelId: sku.ModelId.Value!, colorEnum: colorEnum);

            imageUrl = await _distributedCache.GetStringAsync(imageCacheKey, cancellationToken);
        }

        var embeddedSku = EmbeddedSku.Create(skuId: SkuId.Of(request.SkuId),
                                             model: sku.Model,
                                             color: sku.Color,
                                             storage: sku.Storage,
                                             imageUrl: imageUrl ?? "");

        var skuRequest = SkuRequest.Create(requestId: RequestId.Create(),
                                           senderUserId: request.SenderUserId,
                                           fromBranch: EmbeddedBranch.Create(fromBranch.Id, fromBranch.Name),
                                           toBranch: EmbeddedBranch.Create(toBranch.Id, toBranch.Name),
                                           sku: embeddedSku,
                                           requestQuantity: request.RequestQuantity,
                                           state: ESkuRequestState.PENDING);

        var result = await _skuRequestRepository.InsertOneAsync(skuRequest);

        if (result.IsFailure)
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error]::: Failed to create SKU request. Error: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(CreateSkuRequestHandler), result.Error.Message, new { request });

            return result.Error;
        }

        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Successfully created SKU request. SKU Request ID: {SkuRequestId}",
            nameof(CreateSkuRequestHandler), skuRequest.Id.Value);

        return true;
    }
}
