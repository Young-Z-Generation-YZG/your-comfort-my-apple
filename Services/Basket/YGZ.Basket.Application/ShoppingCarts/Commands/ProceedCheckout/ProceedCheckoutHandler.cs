using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.ProceedCheckout;

public class ProceedCheckoutHandler : ICommandHandler<ProceedCheckoutCommand, bool>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUserHttpContext _userContext;
    private readonly ILogger<ProceedCheckoutHandler> _logger;

    public ProceedCheckoutHandler(IBasketRepository basketRepository, IUserHttpContext userContext, ILogger<ProceedCheckoutHandler> logger)
    {
        _basketRepository = basketRepository;
        _userContext = userContext;
        _logger = logger;
    }


    public async Task<Result<bool>> Handle(ProceedCheckoutCommand request, CancellationToken cancellationToken)
    {
        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.Response is null || !result.Response.CartItems.Any())
        {
            return Errors.Basket.BasketEmpty;
        }

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            return Errors.Basket.BasketEmpty;
        }

        ShoppingCart checkoutCart = shoppingCart.RemoveEventItems();

        if (checkoutCart.CartItems.All(ci => ci.IsSelected == false))
        {
            return Errors.Basket.NotSelectedItems;
        }

        await _basketRepository.StoreBasketAsync(checkoutCart, cancellationToken);


        return true;
    }
}
