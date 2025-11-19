using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Api.Protos;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using YGZ.Discount.Domain.Event;
using EventItemEntity = YGZ.Discount.Domain.Event.Entities.EventItem;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Commands.UpdateEvent;

public class UpdateEventHandler : ICommandHandler<UpdateEventCommand, bool>
{
    private readonly ILogger<UpdateEventHandler> _logger;
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;
    private readonly IDistributedCache _distributedCache;

    public UpdateEventHandler(ILogger<UpdateEventHandler> logger,
                              IGenericRepository<Event, ValueObjects.EventId> repository,
                              CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient,
                              IDistributedCache distributedCache)
    {
        _logger = logger;
        _repository = repository;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _distributedCache = distributedCache;
    }

    public async Task<Result<bool>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventId = ValueObjects.EventId.Of(request.EventId);
        var eventEntity = await _repository.DbSet
            .Include(e => e.EventItems)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
        {
            _logger.LogError("Event with ID {EventId} not found.", request.EventId);
            return Errors.Event.EventNotFound;
        }

        // Update event properties (only if provided)
        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            eventEntity.Title = request.Title;
        }

        if (request.Description is not null)
        {
            eventEntity.Description = request.Description;
        }

        if (request.StartDate.HasValue)
        {
            eventEntity.StartDate = request.StartDate.Value;
        }

        if (request.EndDate.HasValue)
        {
            eventEntity.EndDate = request.EndDate.Value;
        }

        // Remove event items
        if (request.RemoveEventItemIds is not null && request.RemoveEventItemIds.Any())
        {
            foreach (var eventItemIdStr in request.RemoveEventItemIds)
            {
                var eventItemId = ValueObjects.EventItemId.Of(eventItemIdStr);
                var eventItem = eventEntity.EventItems.FirstOrDefault(ei => ei.Id == eventItemId);

                if (eventItem is not null)
                {
                    eventEntity.RemoveEventItem(eventItem);
                    _logger.LogInformation("Removed event item {EventItemId} from event {EventId}.", eventItemIdStr, request.EventId);
                }
                else
                {
                    _logger.LogWarning("Event item {EventItemId} not found in event {EventId}.", eventItemIdStr, request.EventId);
                }
            }
        }

        // Add event items
        if (request.AddEventItems is not null && request.AddEventItems.Any())
        {
            foreach (var itemCmd in request.AddEventItems)
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
                        _logger.LogError("SKU with ID {SkuId} not found.", itemCmd.SkuId);
                        continue; // Skip this item
                    }

                    // Validate stock availability
                    if (skuResponse.AvailableInStock < itemCmd.Stock)
                    {
                        _logger.LogWarning("SKU with ID {SkuId} has not enough stock. Available: {AvailableInStock}, Requested: {Stock}",
                            itemCmd.SkuId, skuResponse.AvailableInStock, itemCmd.Stock);
                        continue; // Skip this item
                    }

                    // Convert enums
                    var productClassification = EProductClassification.FromName(skuResponse.ProductClassification, false);
                    var discountType = EDiscountType.FromName(itemCmd.DiscountType, false);
                    var iphoneModelEnum = EIphoneModel.FromName(skuResponse.NormalizedModel);
                    var colorEnum = EColor.FromName(skuResponse.NormalizedColor);
                    var storageEnum = EStorage.FromName(skuResponse.NormalizedStorage);

                    // Check if event item with same SKU already exists
                    var existingItemBySku = eventEntity.EventItems.FirstOrDefault(ei => 
                        ei.SkuId == itemCmd.SkuId && !ei.IsDeleted);

                    if (existingItemBySku is not null)
                    {
                        _logger.LogWarning("Event item with SKU {SkuId} already exists in event {EventId}. Skipping.", 
                            itemCmd.SkuId, request.EventId);
                        continue; // Skip this item
                    }

                    // Calculate original price from SKU unit price
                    var originalPrice = skuResponse.UnitPrice.HasValue ? (decimal)skuResponse.UnitPrice.Value : 0;

                    var colorImageUrl = await _distributedCache.GetStringAsync(
                        CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(skuResponse.ModelId, colorEnum),
                        cancellationToken);
                        
                    // Create event item with data from SKU
                    var eventItem = EventItemEntity.Create(eventItemId: ValueObjects.EventItemId.Create(),
                                                           eventId: eventId,
                                                           skuId: itemCmd.SkuId,
                                                           tenantId: skuResponse.TenantId,
                                                           branchId: skuResponse.BranchId,
                                                           iphoneModelEnum: iphoneModelEnum,
                                                           colorEnum: colorEnum,
                                                           storageEnum: storageEnum,
                                                           productClassification: productClassification,
                                                           discountType: discountType,
                                                           colorHexCode: skuResponse.ColorHexCode,
                                                           imageUrl: colorImageUrl ?? string.Empty,
                                                           discountValue: itemCmd.DiscountValue,
                                                           originalPrice: originalPrice,
                                                           stock: itemCmd.Stock);

                    eventEntity.AddEventItem(eventItem);
                    _logger.LogInformation("Added event item for SKU {SkuId} to event {EventId}.", itemCmd.SkuId, request.EventId);
                }
                catch (RpcException ex)
                {
                    _logger.LogError(ex, "Error adding event item for SKU {SkuId}", itemCmd.SkuId);
                    // Continue with other items
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error adding event item for SKU {SkuId}", itemCmd.SkuId);
                    // Continue with other items
                }
            }
        }

        // Update timestamp
        eventEntity.UpdatedAt = DateTime.UtcNow;

        // Save all changes atomically
        await _repository.UpdateAsync(eventEntity, cancellationToken);

        _logger.LogInformation("Updated event {EventId} with property changes and item modifications.", request.EventId);

        return true;
    }
}
