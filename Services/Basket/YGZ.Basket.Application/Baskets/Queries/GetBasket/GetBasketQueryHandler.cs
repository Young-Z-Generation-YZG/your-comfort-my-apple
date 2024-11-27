
using YGZ.Basket.Application.Contracts;
using YGZ.Basket.Application.Core.Abstractions.Messaging;
using YGZ.Basket.Domain.Core.Abstractions.Result;

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
            return new GetBasketResponse(null, request.UserId, [], null, null, null);
        }

        var response = new GetBasketResponse(
            basket.Response!.Id.Value.ToString(),
            basket.Response.UserId.Value.ToString(),
            basket.Response.CartLines.ConvertAll(line => new CartLineResponse(line.ProductItemId,
                                                                              line.Model,
                                                                              line.Color,
                                                                              line.Storage,
                                                                              line.Quantity,
                                                                              line.Price,
                                                                              line.SubTotal)),
            basket.Response.Total,
            basket.Response.CreatedAt,
            basket.Response.UpdatedAt);

        return response;
    }
}
