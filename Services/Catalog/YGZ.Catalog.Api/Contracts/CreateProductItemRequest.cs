using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts;

public sealed record CreateProductItemRequest(
    [property: JsonPropertyName("model")] string Model,
    [property: JsonPropertyName("color")] string Color,
    [property: JsonPropertyName("storage")] int Storage,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("quantity_in_stock")] int QuantityInStock,
    [property: JsonPropertyName("product_id")] string ProductId)
{ }