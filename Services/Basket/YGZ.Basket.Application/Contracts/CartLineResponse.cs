
namespace YGZ.Basket.Application.Contracts;

public sealed record CartLineResponse(
    string ProductId,
    string Sku,
    string Model,
    string Color,
    string Storage,
    int Quantity,
    decimal Price,
    string Image_url,
    decimal Sub_total) { }

