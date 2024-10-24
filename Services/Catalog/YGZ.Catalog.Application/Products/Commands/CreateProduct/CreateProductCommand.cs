
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductCommand : ICommand<bool> {
    public string Name { get; set; }
}