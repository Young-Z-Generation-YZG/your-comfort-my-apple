
using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetBasket;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserContext _userContext;

    public GetBasketQueryHandler(IBasketRepository basketRepository, IUserContext userContext)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        //var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync("lov3rinve146@gmail.com", cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var response = new GetBasketResponse
        {
            Cart = result.Response!.Items.Select(x => new CartItemResponse(x.ProductId,
                                                                           x.ProductModel,
                                                                           x.ProductColor,
                                                                           x.ProductColorHex,
                                                                           x.ProductStorage,
                                                                           x.ProductPrice,
                                                                           x.ProductImage,
                                                                           x.Quantity)).ToList()
        };

        return response;
    }
}