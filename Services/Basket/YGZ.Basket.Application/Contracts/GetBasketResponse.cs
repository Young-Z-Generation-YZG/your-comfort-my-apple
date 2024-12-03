

using YGZ.Basket.Domain.ShoppingCart;

namespace YGZ.Basket.Application.Contracts;

public sealed record GetBasketResponse(string? CartId,
                                       string UserId,
                                       List<CartLineResponse> Cart_lines,
                                       double? Total,
                                       DateTime? Created_at,
                                       DateTime? Updated_at) { }


