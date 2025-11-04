
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Inventory.Commands.UpdateSkuCommand;

public sealed record DeductQuantityCommand : ICommand<bool>
{
    public required OrderConfirmedIntegrationEvent Order { get; init; }
}
