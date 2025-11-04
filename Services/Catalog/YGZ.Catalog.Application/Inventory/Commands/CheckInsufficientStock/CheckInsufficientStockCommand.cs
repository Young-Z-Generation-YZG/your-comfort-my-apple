using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Enums;

namespace YGZ.Catalog.Application.Inventory.Commands.CheckInsufficientStock;

public sealed record CheckInsufficientStockCommand : ICommand<bool>
{
    public required string ModelId { get; init; }
    public required string NormalizedModel { get; init; }
    public required string NormalizedStorage { get; init; }
    public required string NormalizedColor { get; init; }
    public required int Quantity { get; init; }
    public string? PromotionId { get; init; }
    public EPromotionType PromotionType { get; init; }
}