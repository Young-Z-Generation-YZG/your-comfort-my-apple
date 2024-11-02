
namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductResponse(string Id,
                                           string Name,
                                           List<ProductItemResponse> ProductItems,
                                           string? CategoryId,
                                           string? PromotionId,
                                           DateTime Created_at,
                                           DateTime Updated_at) { }

public sealed record ProductItemResponse(string Id, string Name, string Description, decimal Price, InventoryResponse Inventory) { }

public sealed record InventoryResponse(string Id, int QuantityInStock) { }
