using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.StoreEventItem;

public sealed record StoreEventItemCommand : ICommand<bool>
{
    public required string EventItemId { get; init; }
    public required ProductInformationCommand ProductInformation { get; init; }
}

public sealed record ProductInformationCommand
{
    public required string ProductName { get; init; }
    public required string ModelNormalizedName { get; init; }
    public required string ColorNormalizedName { get; init; }
    public required string StorageNormalizedName { get; init; }
    public required string DisplayImageUrl { get; init; }
}
