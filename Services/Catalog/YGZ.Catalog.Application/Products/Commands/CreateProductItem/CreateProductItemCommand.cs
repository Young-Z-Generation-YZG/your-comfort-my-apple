﻿
using YGZ.Catalog.Application.Common.Commands;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;

namespace YGZ.Catalog.Application.Products.Commands.CreateProductItem;

public sealed record CreateProductItemCommand : ICommand<bool>
{
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int Storage { get; set; } = 0;
    public double Price { get; set; } = 0;
    public int Quantity_in_stock { get; set; }
    public List<ImageCommand> Images { get; set; } = new();
    public string ProductId { get; set; } = default!;
}