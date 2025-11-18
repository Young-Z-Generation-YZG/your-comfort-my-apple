using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Promotions.Events.UpdateEvent;

public sealed record UpdateEventCommand : ICommand<bool>
{
    public required string EventId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<UpdateEventItemCommand>? AddEventItems { get; init; }
    public List<string>? RemoveEventItemIds { get; init; }
}

public sealed record UpdateEventItemCommand
{
    public required string SkuId { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required int Stock { get; init; }
}
