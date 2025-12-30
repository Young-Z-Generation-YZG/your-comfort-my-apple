using Grpc.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.ValueObjects;
using EventItemEntity = YGZ.Discount.Domain.Event.Entities.EventItem;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Commands.AddEventItem;

public class AddEventItemsHandler : ICommandHandler<AddEventItemsCommand, bool>
{
    private readonly ILogger<AddEventItemsHandler> _logger;
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;
    private readonly IDistributedCache _distributedCache;

    public AddEventItemsHandler(IGenericRepository<Event, ValueObjects.EventId> repository,
                                CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient,
                                IDistributedCache distributedCache,
                                ILogger<AddEventItemsHandler> logger)
    {
        _repository = repository;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AddEventItemsCommand request, CancellationToken cancellationToken)
    {
        if (request.DiscountEventItems is null || !request.DiscountEventItems.Any())
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "No discount event items provided in request", new { eventId = request.EventId });

            return false;
        }

        // Get the event
        var eventId = ValueObjects.EventId.Of(request.EventId);
        var eventEntity = await _repository.GetByIdAsync(eventId, cancellationToken);

        if (eventEntity is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.GetByIdAsync), "Event not found", new { eventId = request.EventId });

            return false;
        }

        // Create and add event items
        foreach (var itemCmd in request.DiscountEventItems)
        {
            try
            {
                // Fetch SKU details from Catalog service
                var skuResponse = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                {
                    SkuId = itemCmd.SkuId
                }, cancellationToken: cancellationToken);

                if (skuResponse is null)
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), "SKU not found", new { eventId = request.EventId, skuId = itemCmd.SkuId });

                    throw new RpcException(new Status(StatusCode.NotFound, $"SKU with ID {itemCmd.SkuId} not found"));
                }

                // Validate stock availability
                if (skuResponse.AvailableInStock < itemCmd.Stock)
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(Handle), "SKU has insufficient stock", new { eventId = request.EventId, skuId = itemCmd.SkuId, availableStock = skuResponse.AvailableInStock, requestedStock = itemCmd.Stock });

                    throw new RpcException(new Status(StatusCode.FailedPrecondition,
                        $"SKU with ID {itemCmd.SkuId} has not enough stock. Available: {skuResponse.AvailableInStock}, Requested: {itemCmd.Stock}"));
                }

                // Convert enums
                var productClassification = EProductClassification.FromName(skuResponse.ProductClassification, false);
                var discountType = EDiscountType.FromName(itemCmd.DiscountType, false);

                // Calculate original price from SKU unit price
                var originalPrice = skuResponse.UnitPrice.HasValue ? (decimal)skuResponse.UnitPrice.Value : 0;

                var imageUrl = await _distributedCache.GetStringAsync(CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(skuResponse.ModelId, EColor.FromName(skuResponse.NormalizedColor)), cancellationToken);

                // Create event item with data from SKU
                var eventItem = EventItemEntity.Create(eventItemId: EventItemId.Create(),
                                                       eventId: eventId,
                                                       skuId: itemCmd.SkuId,
                                                       tenantId: skuResponse.TenantId,
                                                       branchId: skuResponse.BranchId,
                                                       iphoneModelEnum: EIphoneModel.FromName(skuResponse.NormalizedModel),
                                                       colorEnum: EColor.FromName(skuResponse.NormalizedColor),
                                                       storageEnum: EStorage.FromName(skuResponse.NormalizedStorage),
                                                       productClassification: productClassification,
                                                       discountType: discountType,
                                                       colorHexCode: skuResponse.ColorHexCode,
                                                       imageUrl: imageUrl ?? string.Empty,
                                                       discountValue: itemCmd.DiscountValue,
                                                       originalPrice: originalPrice,
                                                       stock: itemCmd.Stock);

                eventEntity.AddEventItem(eventItem);
            }
            catch (RpcException ex)
            {
                var parameters = new { eventId = request.EventId, skuId = itemCmd.SkuId };
                _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_catalogProtoServiceClient.GetSkuByIdGrpcAsync), ex.Message, parameters);
                throw;
            }
        }

        // Save changes
        var updateResult = await _repository.UpdateAsync(eventEntity, cancellationToken);

        if (updateResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to update event with new event items", new { eventId = request.EventId, itemCount = request.DiscountEventItems.Count, error = updateResult.Error });

            return updateResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully added event items to event", new { eventId = request.EventId, itemCount = request.DiscountEventItems.Count });

        return true;
    }
}
