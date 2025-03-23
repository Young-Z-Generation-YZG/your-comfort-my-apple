using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public sealed record GetBasketQuery(string? CouponCode) : IQuery<GetBasketResponse> { }

