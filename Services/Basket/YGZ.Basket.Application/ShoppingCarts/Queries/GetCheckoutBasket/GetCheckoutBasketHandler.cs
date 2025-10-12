using YGZ.Basket.Application.Abstractions;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.Discount.Grpc.Protos;

namespace YGZ.Basket.Application.ShoppingCarts.Queries.GetCheckoutBasket;

public class GetCheckoutBasketHandler : IQueryHandler<GetCheckoutBasketQuery, GetBasketResponse>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserRequestContext _userContext;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

    public GetCheckoutBasketHandler(IBasketRepository basketRepository, IUserRequestContext userContext, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
    {
        _discountProtoServiceClient = discountProtoServiceClient;
        _basketRepository = basketRepository;
        _userContext = userContext;
    }

    public async Task<Result<GetBasketResponse>> Handle(GetCheckoutBasketQuery request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            return new GetBasketResponse()
            {
                UserEmail = shoppingCart.UserEmail,
                CartItems = new List<CartItemResponse>(),
                TotalAmount = 0
            };
        }

        if (shoppingCart.CheckHasEventItems())
        {
            ShoppingCart FilterEventItemsShoppingCart = shoppingCart.FilterEventItems();

            return new GetBasketResponse()
            {
                UserEmail = FilterEventItemsShoppingCart.UserEmail,
                CartItems = FilterEventItemsShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                TotalAmount = FilterEventItemsShoppingCart.TotalAmount
            };
        }
        else
        {
            ShoppingCart FilterOutEventItemsShoppingCart = shoppingCart.FilterOutEventItemsAndSelected();

            return new GetBasketResponse()
            {
                UserEmail = FilterOutEventItemsShoppingCart.UserEmail,
                CartItems = FilterOutEventItemsShoppingCart.CartItems.Select(item => item.ToResponse()).ToList(),
                TotalAmount = FilterOutEventItemsShoppingCart.TotalAmount
            };
        }
    }
}
