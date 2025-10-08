namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

public sealed record EventWithEventItemsResponse
{
    public required EventResponse Event { get; init; }
    public required List<EventItemResponse> EventItems { get; init; }
}
