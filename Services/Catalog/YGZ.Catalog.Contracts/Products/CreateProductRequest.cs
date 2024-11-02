using Microsoft.AspNetCore.Http;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductRequest(string Name, List<string> Image_urls, List<string> Image_ids, List<ProductItem> Product_items) { }

public sealed record ProductItem(string Name, string Description, decimal Price, Inventory Inventory) { }

public sealed record Inventory(int Quantity_in_stock) { }
