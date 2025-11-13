
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Inventory.Commands.UpdateSkuCommand;

public class DeductQuantityHandler : ICommandHandler<DeductQuantityCommand, bool>
{
    private readonly ILogger<DeductQuantityHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _repository;

    public DeductQuantityHandler(ILogger<DeductQuantityHandler> logger, IMongoRepository<SKU, SkuId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeductQuantityCommand request, CancellationToken cancellationToken)
    {
        foreach (var orderItem in request.Order.OrderItems)
        {
            var sku = await _repository.GetByIdAsync(orderItem.SkuId, cancellationToken);

            if (sku is null)
            {
                return Errors.Inventory.SkuDoesNotExist;
            }

            if (sku.AvailableInStock < orderItem.Quantity)
            {
                return Errors.Inventory.QuantityIsLessThanTheQuantityToDeduct;
            }


            if (orderItem.PromotionType == EPromotionType.EVENT.Name)
            {
                sku.DeductReservedQuantity(orderItem.Quantity);
            }
            else
            {
                sku.DeductQuantity(orderItem.Quantity);
            }

            await _repository.UpdateAsync(sku.Id.Value!, sku);
        }

        return true;
    }
}
