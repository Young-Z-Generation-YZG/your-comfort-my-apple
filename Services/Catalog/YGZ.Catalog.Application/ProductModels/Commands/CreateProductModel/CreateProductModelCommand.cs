using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductModelCommand() : ICommand<bool>
{
}
