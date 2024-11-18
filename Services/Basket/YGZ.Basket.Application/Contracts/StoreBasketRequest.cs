

namespace YGZ.Basket.Application.Contracts;

public sealed record StoreBasketRequest(string UserId, List<CartLineRequest> Cart_lines) { }
