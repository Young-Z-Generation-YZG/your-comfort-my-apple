namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

public sealed record RevenuesByGroupResponse
{
    public required Dictionary<string, List<OrderDetailsResponse>> Groups { get; init; }
}
