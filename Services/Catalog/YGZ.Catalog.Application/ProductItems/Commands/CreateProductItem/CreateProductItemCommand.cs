using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.Catalog.Application.Common.Commands;

namespace YGZ.Catalog.Application.ProductItems.Commands.CreateProductItem;

public sealed record CreateProductItemCommand() : ICommand<bool>
{
    public string Model { get; set; } = default!;
    public ColorCommand Color { get; set; } = default!;
    public int Storage { get; set; } = 0;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; } = 0;
    public ImageCommand[] Images { get; set; } = default!;
    public string ProductId { get; set; } = default!;
}
