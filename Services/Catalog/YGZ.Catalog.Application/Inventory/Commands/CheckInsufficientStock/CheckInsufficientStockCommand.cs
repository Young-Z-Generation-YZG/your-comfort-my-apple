using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Catalog.Application.Inventory.Commands.CheckInsufficientStock;

public sealed record CheckInsufficientStockCommand : ICommand<bool>
{
    public required string SkuId { get; init; }
    public required int Quantity { get; init; }
    public string? PromotionId { get; init; }
    public EPromotionType PromotionType { get; init; }
}