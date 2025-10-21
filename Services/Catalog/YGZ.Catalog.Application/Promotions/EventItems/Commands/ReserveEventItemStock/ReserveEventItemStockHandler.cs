using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Promotions.EventItems.Commands.ReserveEventItemStock;

public class ReserveEventItemStockHandler : ICommandHandler<ReserveEventItemStockCommand, bool>
{
    private readonly IMongoRepository<SKU, SkuId> _repository;
    private readonly ILogger<ReserveEventItemStockHandler> _logger;

    public ReserveEventItemStockHandler(IMongoRepository<SKU, SkuId> repository, ILogger<ReserveEventItemStockHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(ReserveEventItemStockCommand request, CancellationToken cancellationToken)
    {
        var sku = await _repository.GetByIdAsync(request.SkuId, cancellationToken);

        if (sku is null)
        {
            _logger.LogError("SKU not found with ID {EventItemId}", request.EventItemId);

            return false;
        }

        sku.SetReservedForEvent(request.EventId, request.EventItemId, request.EventName, request.ReservedQuantity);

        await _repository.UpdateAsync(sku.Id.Value!, sku);

        return true;
    }
}
