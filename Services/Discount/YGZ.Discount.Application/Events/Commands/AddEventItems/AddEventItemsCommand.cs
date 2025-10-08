using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Events.Commands.AddEventItem;

public class AddEventItemsCommand() : ICommand<bool>
{
    public required string EventId { get; init; }
    public required List<EventItemCommand> EventItems { get; init; }
}

public class EventItemCommand
{
    public required ModelCommand Model { get; init; }
    public required ColorCommand Color { get; init; }
    public required StorageCommand Storage { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required string ProductType { get; set; }
    public required string DiscountType { get; set; }
    public required decimal DiscountValue { get; set; }
    public required decimal OriginalPrice { get; set; }
    public required int Stock { get; set; }
}

public class ModelCommand
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
}

public class ColorCommand
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
    public required string HexCode { get; set; }
}

public class StorageCommand
{
    public required string Name { get; set; }
    public required string NormalizedName { get; set; }
}