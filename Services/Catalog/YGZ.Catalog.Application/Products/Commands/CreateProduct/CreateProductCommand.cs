
using Microsoft.AspNetCore.Http;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : ICommand<bool> {
    public string Name { get; set; } = string.Empty;

    //public IFormFile[] Files { get; set; } = [];
    public string[] Image_urls { get; set; } = [];
    public string[] Image_ids { get; set; } = [];
    public List<ProductItemCommand> Product_items { get; set; } = [];
}

public sealed record ProductItemCommand(string Name, string Description, decimal Price, InventoryCommand Inventory) { }

public sealed record InventoryCommand(int Quantity_in_stock) { }