

using YGZ.Basket.Domain.ShoppingCart;

namespace YGZ.Basket.Application.Contracts;

public sealed record GetBasketResponse(string cart_id,
                                       string user_id,
                                       List<CartLineResponse> cart_lines,
                                       decimal total,
                                       DateTime created_at,
                                       DateTime updated_at) { }


