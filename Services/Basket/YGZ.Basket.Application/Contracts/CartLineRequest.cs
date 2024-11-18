

namespace YGZ.Basket.Application.Contracts;

public sealed record CartLineRequest(
    string ProductId,
    string Sku,
    string Model,
    string Color,
    string Storage,
    int Price,
    int Quantity,
    string Image_url) { }
