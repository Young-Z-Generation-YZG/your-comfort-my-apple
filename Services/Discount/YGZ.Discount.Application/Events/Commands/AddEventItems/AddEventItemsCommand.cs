using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Events.Commands.AddEventItem;

public class AddEventItemsCommand() : ICommand<bool>
{
    public required string EventId { get; init; }
    public required List<EventItemCommand> EventItems { get; init; }
}

public class EventItemCommand
{
    public required string SkuId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public required ModelCommand Model { get; init; }
    public required ColorCommand Color { get; init; }
    public required StorageCommand Storage { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required string ProductClassification { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal OriginalPrice { get; init; }
    public required int Stock { get; init; }
}

public class ModelCommand
{
    public required string Name { get; init; }
    public required string NormalizedName { get; init; }
}

public class ColorCommand
{
    public required string Name { get; init; }
    public required string NormalizedName { get; init; }
    public required string HexCode { get; init; }
}

public class StorageCommand
{
    public required string Name { get; init; }
    public required string NormalizedName { get; init; }
}