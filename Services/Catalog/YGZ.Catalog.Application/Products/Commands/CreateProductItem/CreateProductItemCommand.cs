
using YGZ.Catalog.Application.Common.Commands;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public sealed record CreateProductItemCommand : ICommand<bool>
{
    public string Model { get; set; } = default!;
    public string Color { get; set; } = default!;
    public int Storage { get; set; } = 0;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; } = 0;
    public int Quantity_in_stock { get; set; }
    public List<ImageCommand> Images { get; set; } = new();
    public string ProductId { get; set; } = default!;
    public string PromotionId { get; set; } = default!;
}