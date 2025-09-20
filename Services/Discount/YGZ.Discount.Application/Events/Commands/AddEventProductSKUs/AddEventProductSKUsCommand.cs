using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Discount.Application.Events.Commands.AddEventProductSKUs;

public sealed record AddEventProductSKUsCommand(List<EventProductSKUCommand> EventProductSKUs) : ICommand<bool> { }

public sealed record EventProductSKUCommand
{
    public required string EventId { get; set; }
    public required string SkuId { get; set; }
    public required string ModelId { get; set; }
    public required string ModelName { get; set; }
    public required string StorageName { get; set; }
    public required string ColorHaxCode { get; set; }
    public required string ImageUrl { get; set; }
    public required string ProductType { get; set; }
    public required string DiscountType { get; set; }
    public required decimal DiscountValue { get; set; }
    public required decimal OriginalPrice { get; set; }
    public required int Stock { get; set; }
}
