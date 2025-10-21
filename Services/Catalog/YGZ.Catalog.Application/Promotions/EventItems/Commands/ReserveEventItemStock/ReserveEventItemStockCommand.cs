using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Promotions.EventItems.Commands.ReserveEventItemStock;

public sealed record ReserveEventItemStockCommand : ICommand<bool>
{
    public required string SkuId { get; init; }
    public required string EventId { get; init; }
    public required string EventItemId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public required string EventName { get; init; }
    public required int ReservedQuantity { get; init; }
}
