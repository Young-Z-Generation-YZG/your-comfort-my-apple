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
        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(ProceedCheckoutHandler), "Start proceeding checkout...", request);

        var userEmail = _userContext.GetUserEmail();
        var userId = _userContext.GetUserId();

        var result = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:IsFailure][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(ProceedCheckoutHandler), nameof(_basketRepository.GetBasketAsync), result.Error.Message, new { userEmail, error = result.Error });

            return result.Error;
        }

        var shoppingCart = result.Response!;

        if (shoppingCart is null || !shoppingCart.CartItems.Any())
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(ProceedCheckoutHandler), nameof(_basketRepository.GetBasketAsync), Errors.Basket.BasketEmpty.Message, new { userEmail });

            return Errors.Basket.BasketEmpty;
        }

        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(ProceedCheckoutHandler), "Removing event items from cart...", new { userEmail, totalCartItems = shoppingCart.CartItems.Count });

        ShoppingCart checkoutCart = shoppingCart.RemoveEventItems();

        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(ProceedCheckoutHandler), "Event items removed from cart", new { userEmail, remainingCartItems = checkoutCart.CartItems.Count, removedCount = shoppingCart.CartItems.Count - checkoutCart.CartItems.Count });

        if (checkoutCart.CartItems.All(ci => ci.IsSelected == false))
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:Error][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(ProceedCheckoutHandler), nameof(Handle), Errors.Basket.NotSelectedItems.Message, new { userEmail, cartItemCount = checkoutCart.CartItems.Count });

            return Errors.Basket.NotSelectedItems;
        }

        _logger.LogInformation(":::[CommandHandler:{CommandHandler}]::: Information message: {Message}, Parameters: {@Parameters}",
            nameof(ProceedCheckoutHandler), "Storing checkout cart...", new { userEmail, selectedCartItems = checkoutCart.CartItems.Count(ci => ci.IsSelected) });

        var storeResult = await _basketRepository.StoreBasketAsync(checkoutCart, cancellationToken);

        if (storeResult.IsFailure)
        {
            _logger.LogError(":::[CommandHandler:{CommandHandler}][Result:IsFailure][Method:{MethodName}]::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(ProceedCheckoutHandler), nameof(_basketRepository.StoreBasketAsync), storeResult.Error.Message, new { userEmail, error = storeResult.Error });

            return storeResult.Error;
        }
        return true;
    }
}
