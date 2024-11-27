

namespace YGZ.Basket.Application.Contracts;

public sealed record CartLineRequest(
    string ProductItemId,
    string Model,
    string Color,
    int Storage,
    decimal Price,
    int Quantity) { }
