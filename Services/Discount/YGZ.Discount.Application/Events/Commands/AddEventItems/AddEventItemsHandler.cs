using Grpc.Core;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
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
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;
    private readonly CatalogProtoService.CatalogProtoServiceClient _catalogProtoServiceClient;
    private readonly ILogger<AddEventItemsHandler> _logger;

    public AddEventItemsHandler(IGenericRepository<Event, ValueObjects.EventId> repository,
                                CatalogProtoService.CatalogProtoServiceClient catalogProtoServiceClient,
                                ILogger<AddEventItemsHandler> logger)
    {
        _repository = repository;
        _catalogProtoServiceClient = catalogProtoServiceClient;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AddEventItemsCommand request, CancellationToken cancellationToken)
    {
        if (!request.EventItems.Any())
        {
            _logger.LogError("No EventItems provided in the request.");

            return false;
        }

        // Get the event
        var eventId = ValueObjects.EventId.Of(request.EventId);

        foreach (var item in request.EventItems)
        {
            try
            {
                var checkTenantExist = await _catalogProtoServiceClient.GetTenantByIdGrpcAsync(new GetTenantByIdRequest
                {
                    TenantId = item.TenantId
                }, cancellationToken: cancellationToken);

                if (checkTenantExist is null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Tenant with ID {item.TenantId} not found"));
                }

            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "Error checking tenant existence for tenant {TenantId}", item.TenantId);
                throw new RpcException(new Status(StatusCode.Internal, "Internal error occurred while checking tenant existence"));
            }

        }

        var eventEntity = await _repository.GetByIdAsync(eventId, cancellationToken);

        if (eventEntity is null)
        {
            _logger.LogError("Event with ID {EventId} not found.", request.EventId);

            return false;
        }

        // Create and add event items
        foreach (var itemCmd in request.EventItems)
        {
            try
            {
                var checkEnoughSku = await _catalogProtoServiceClient.GetSkuByIdGrpcAsync(new GetSkuByIdRequest
                {
                    SkuId = itemCmd.SkuId
                }, cancellationToken: cancellationToken);

                if (checkEnoughSku is null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"SKU with ID {itemCmd.SkuId} not found"));
                }

                if (checkEnoughSku.AvailableInStock < itemCmd.Stock)
                {
                    _logger.LogError("SKU with ID {SkuId} has not enough stock. Available: {AvailableInStock}, Requested: {Stock}", itemCmd.SkuId, checkEnoughSku.AvailableInStock, itemCmd.Stock);
                    throw new RpcException(new Status(StatusCode.FailedPrecondition, $"SKU with ID {itemCmd.SkuId} has not enough stock. Available: {checkEnoughSku.AvailableInStock}, Requested: {itemCmd.Stock}"));
                }
            }
            catch (RpcException ex)
            {
                throw;
                //_logger.LogError(ex, "Error checking SKU existence for SKU {SkuId}", itemCmd.SkuId);
                //throw new RpcException(new Status(StatusCode.Internal, "Internal error occurred while checking SKU existence"));
            }

            var productClassification = EProductClassification.FromName(itemCmd.ProductClassification, false);
            var discountType = EDiscountType.FromName(itemCmd.DiscountType, false);

            var eventItem = EventItemEntity.Create(eventItemId: EventItemId.Create(),
                                                   eventId: eventId,
                                                   skuId: itemCmd.SkuId,
                                                   tenantId: itemCmd.TenantId,
                                                   branchId: itemCmd.BranchId,
                                                   modelName: itemCmd.Model.Name,
                                                   normalizedModel: itemCmd.Model.NormalizedName,
                                                   colorName: itemCmd.Color.Name,
                                                   normalizedColor: itemCmd.Color.NormalizedName,
                                                   colorHaxCode: itemCmd.Color.HexCode,
                                                   storageName: itemCmd.Storage.Name,
                                                   normalizedStorage: itemCmd.Storage.NormalizedName,
                                                   productClassification: productClassification,
                                                   discountType: discountType,
                                                   imageUrl: itemCmd.DisplayImageUrl,
                                                   discountValue: itemCmd.DiscountValue,
                                                   originalPrice: itemCmd.OriginalPrice,
                                                   stock: itemCmd.Stock);

            eventEntity.AddEventItem(eventItem);
        }

        // Save changes
        await _repository.UpdateAsync(eventEntity, cancellationToken);

        _logger.LogInformation("Added {Count} event items to event {EventId}", request.EventItems.Count, request.EventId);

        return true;
    }
}
