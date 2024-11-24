
using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Application.Baskets.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketByUserIdQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;

    public GetBasketQueryHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }


    public async Task<Result<GetBasketResponse>> Handle(GetBasketByUserIdQuery request, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(request.UserId);

        if (basket.IsFailure)
        {
            return basket.Error;
        }

        var response = new GetBasketResponse(
            basket.Response.Id.Value.ToString(),
            basket.Response.UserId.Value.ToString(),
            basket.Response.CartLines.ConvertAll(line => new CartLineResponse(line.ProductId,
                                                                              line.Sku,
                                                                              line.Model,
                                                                              line.Color,
                                                                              line.Storage,
                                                                              line.Quantity,
                                                                              line.Price,
                                                                              line.Image_url,
                                                                              line.SubTotal)),
            basket.Response.Total,
            basket.Response.CreatedAt,
            basket.Response.UpdatedAt);

        return response;
    }
}
