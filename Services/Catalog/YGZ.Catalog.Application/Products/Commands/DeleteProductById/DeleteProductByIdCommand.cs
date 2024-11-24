

using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.DeleteProductById;

public sealed record DeleteProductByIdCommand(string Id) : ICommand<bool> { }

