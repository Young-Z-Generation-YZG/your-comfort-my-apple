using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetCheckoutBasket;

public sealed record GetCheckoutBasketQuery(string? CouponCode) : IQuery<GetBasketResponse> { }