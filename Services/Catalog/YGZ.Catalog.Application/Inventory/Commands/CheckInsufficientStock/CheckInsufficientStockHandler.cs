using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Inventory.Commands.CheckInsufficientStock;

public class CheckInsufficientStockHandler : ICommandHandler<CheckInsufficientStockCommand, bool>
{
    private readonly ILogger<CheckInsufficientStockHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _repository;

    public CheckInsufficientStockHandler(ILogger<CheckInsufficientStockHandler> logger, IMongoRepository<SKU, SkuId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CheckInsufficientStockCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<SKU>.Filter.And(
            Builders<SKU>.Filter.Eq("model_id", new ObjectId(request.ModelId)),
            Builders<SKU>.Filter.Eq("model.normalized_name", request.NormalizedModel),
            Builders<SKU>.Filter.Eq("storage.normalized_name", request.NormalizedStorage),
            Builders<SKU>.Filter.Eq("color.normalized_name", request.NormalizedColor));

        var sku = await _repository.GetByFilterAsync(filter, cancellationToken);

        if (sku is null)
        {
            return Errors.Inventory.SkuDoesNotExist;
        }

        // For EVENT promotions, also check reserved quantity
        if (request.PromotionType == EPromotionType.EVENT_ITEM)
        {
            if (sku.ReservedForEvent is null)
            {
                return true; // Stock is insufficient for event
            }

            if (sku.ReservedForEvent.EventItemId != request.PromotionId)
            {
                return true; // Stock is insufficient for this event
            }

            if (sku.ReservedForEvent.ReservedQuantity < request.Quantity)
            {
                return true; // Reserved stock is insufficient
            }
        }

        // Check if stock is insufficient for regular orders
        if (sku.AvailableInStock < request.Quantity)
        {
            return true; // Stock is insufficient
        }

        return false; // Stock is sufficient
    }
}