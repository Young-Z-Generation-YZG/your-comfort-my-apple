
namespace YGZ.Basket.Application.Contracts;

public sealed record CartLineResponse(
    string ProductId,
    string Model,
    string Color,
    int Storage,
    int Quantity,
    double Price,
    double Sub_total) { }

