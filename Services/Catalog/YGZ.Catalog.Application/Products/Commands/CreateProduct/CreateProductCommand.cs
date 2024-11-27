
using YGZ.Catalog.Application.Common.Commands;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : ICommand<bool> {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ImageCommand> Images { get; set; }
    public List<ModelCommand> Models { get; set; }
    public List<ColorCommand> Colors { get; set; }
    public List<int> Storages { get; set; }
    public string CategoryId { get; set; }
    public string PromotionId { get; set; }
}

public sealed record ModelCommand(string Name, int Order) { }
public sealed record ColorCommand(string Name, string ColorHash, string ImageColorUrl, int Order) { }