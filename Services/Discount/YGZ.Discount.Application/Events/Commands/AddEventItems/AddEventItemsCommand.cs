using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Events.Commands.AddEventItem;

public class AddEventItemsCommand() : ICommand<bool>
{
    public required string EventId { get; init; }
    public required List<DiscountEventItemCommand> DiscountEventItems { get; init; }
}

public class DiscountEventItemCommand
{
    public required string SkuId { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required int Stock { get; init; }
}