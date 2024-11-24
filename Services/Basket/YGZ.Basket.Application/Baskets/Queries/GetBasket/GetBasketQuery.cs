

using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Application.Core.Abstractions.Messaging;

namespace YGZ.Basket.Application.Baskets.Queries.GetBasket;

public sealed record GetBasketByUserIdQuery(string UserId) : IQuery<GetBasketResponse> { }

