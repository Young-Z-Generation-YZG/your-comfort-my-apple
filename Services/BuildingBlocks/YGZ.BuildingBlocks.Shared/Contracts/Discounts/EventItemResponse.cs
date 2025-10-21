namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

public sealed record EventItemResponse
{
    public required string Id { get; init; }
    public required string EventId { get; init; }
    public required string ModelName { get; init; }
    public required string NormalizedModel { get; init; }
    public required string ColorName { get; init; }
    public required string NormalizedColor { get; init; }
    public required string ColorHexCode { get; init; }
    public required string StorageName { get; init; }
    public required string NormalizedStorage { get; init; }
    public required string ProductClassification { get; init; }
    public required string ImageUrl { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal OriginalPrice { get; init; }
    public required int Stock { get; init; }
    public required int Sold { get; init; }
}
