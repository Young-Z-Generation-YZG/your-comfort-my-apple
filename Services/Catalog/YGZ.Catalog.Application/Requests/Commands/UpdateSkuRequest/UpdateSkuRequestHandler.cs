using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Requests.SkuRequest;
using YGZ.Catalog.Domain.Requests.SkuRequest.ValueObjects;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Requests.Commands.UpdateSkuRequest;

public class UpdateSkuRequestHandler : ICommandHandler<UpdateSkuRequestCommand, bool>
{
    private readonly ILogger<UpdateSkuRequestHandler> _logger;
    private readonly IMongoRepository<SkuRequest, RequestId> _skuRequestRepository;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;

    public UpdateSkuRequestHandler(IMongoRepository<SkuRequest, RequestId> skuRequestRepository, IMongoRepository<SKU, SkuId> skuRepository, ILogger<UpdateSkuRequestHandler> logger)
    {
        _skuRequestRepository = skuRequestRepository;
        _skuRepository = skuRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateSkuRequestCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Processing update SKU request command. Parameters: {@Parameters}",
            nameof(UpdateSkuRequestHandler), request);

        // Parse State using SmartEnum name matching
        ESkuRequestState.TryFromName(request.State, out var SkuRequestStateEnum);

        if (SkuRequestStateEnum is null)
        {
            _logger.LogWarning(":::[CommandHandler:{CommandHandler}][Result:Error]::: Invalid SKU request state: {State}",
                nameof(UpdateSkuRequestHandler), request.State);
            return Errors.SkuRequest.InvalidState;
        }

        var skuRequest = await _skuRequestRepository.GetByIdAsync(request.SkuRequestId, cancellationToken);

        if (skuRequest is null)
        {
            _logger.LogWarning(":::[CommandHandler:{CommandHandler}][Result:Error]::: SKU request not found. SKU Request ID: {SkuRequestId}",
                nameof(UpdateSkuRequestHandler), request.SkuRequestId);
            return Errors.SkuRequest.NotFound;
        }


        var filters = Builders<SKU>.Filter.And(
            Builders<SKU>.Filter.Eq("branch_id", new ObjectId(skuRequest.ToBranch.BranchId.Value)),
            Builders<SKU>.Filter.Eq(x => x.Model.NormalizedName, skuRequest.Sku.ModelNormalizedName),
            Builders<SKU>.Filter.Eq(x => x.Color.NormalizedName, skuRequest.Sku.ColorNormalizedName),
            Builders<SKU>.Filter.Eq(x => x.Storage.NormalizedName, skuRequest.Sku.StorageNormalizedName)
        );

        var skuRequestToTenant = await _skuRepository.GetByFilterAsync(filters, cancellationToken);


        if (skuRequestToTenant is null)
        {
            _logger.LogWarning(":::[CommandHandler:{CommandHandler}][Result:Error]::: SKU not found. SKU ID: {SkuId}",
                nameof(UpdateSkuRequestHandler), skuRequest.Sku.SkuId.Value);
            return Errors.Sku.NotFound;
        }


        try
        {
            if (SkuRequestStateEnum.Equals(ESkuRequestState.APPROVED))
            {
                if (skuRequestToTenant.AvailableInStock < skuRequest.RequestQuantity)
                {
                    return Errors.Sku.InsufficientStock;
                }
                skuRequest.Approve();
            }
            else if (SkuRequestStateEnum.Equals(ESkuRequestState.REJECTED))
            {
                skuRequest.Reject();
            }
            else
            {
                // If trying to set to Pending or other?
                return Errors.SkuRequest.InvalidTransition;
            }
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, ":::[CommandHandler:{CommandHandler}][Exception]::: Operation failed. Error: {ErrorMessage}",
                nameof(UpdateSkuRequestHandler), ex.Message);
            return Errors.SkuRequest.OperationFailed;
        }


        var result = await _skuRequestRepository.UpdateAsync(skuRequest.Id.Value!, skuRequest);

        if (result.IsFailure)
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error]::: Failed to update SKU request. Error: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(UpdateSkuRequestHandler), result.Error.Message, new { request });
            return result.Error;
        }

        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Successfully updated SKU request. SKU Request ID: {SkuRequestId}",
            nameof(UpdateSkuRequestHandler), skuRequest.Id.Value);

        return true;
    }
}
