
namespace YGZ.Basket.Application.Contracts;

public record GetBasketResponse(string? CartId,
                                string UserId,
                                List<CartLineResponse> Cart_lines,
                                decimal Total_cost_of_goods,
                                decimal Total_discount_amount,
                                decimal Total_amount,
                                DateTime? Created_at,
                                DateTime? Updated_at) { }


