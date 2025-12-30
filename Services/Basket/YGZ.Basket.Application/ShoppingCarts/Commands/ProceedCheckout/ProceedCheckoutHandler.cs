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

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.GetBasketAsync), "Failed to retrieve basket from repository", new { userEmail, error = result.Error });

            return result.Error;
        }

        if (result.Response is null || !result.Response.CartItems.Any())
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Basket is empty or null", new { userEmail });

            return Errors.Basket.BasketEmpty;
        }

        var shoppingCart = result.Response!;

        if (shoppingCart.CartItems is null || !shoppingCart.CartItems.Any())
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Basket has no cart items", new { userEmail });

            return Errors.Basket.BasketEmpty;
        }

        ShoppingCart checkoutCart = shoppingCart.RemoveEventItems();

        if (checkoutCart.CartItems.All(ci => ci.IsSelected == false))
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "No items selected for checkout", new { userEmail, cartItemCount = checkoutCart.CartItems.Count });

            return Errors.Basket.NotSelectedItems;
        }

        var storeResult = await _basketRepository.StoreBasketAsync(checkoutCart, cancellationToken);

        if (storeResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_basketRepository.StoreBasketAsync), "Failed to store checkout cart", new { userEmail, error = storeResult.Error });

            return storeResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully proceeded checkout", new { userEmail, cartItemCount = checkoutCart.CartItems.Count });

        return true;
    }
}
