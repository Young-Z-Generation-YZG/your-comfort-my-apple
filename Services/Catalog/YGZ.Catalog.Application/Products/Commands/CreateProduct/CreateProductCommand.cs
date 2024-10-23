
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name) : ICommand<bool> { }

