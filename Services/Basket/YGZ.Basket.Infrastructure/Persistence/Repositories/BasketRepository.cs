

using Marten;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Infrastructure.Persistence.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDocumentSession _documentSession;

    public BasketRepository(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
    public async Task<Result<ShoppingCart>> GetBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            var basket = await _documentSession.LoadAsync<ShoppingCart>(userEmail, cancellationToken);

            if (basket is null)
            {
                return new ShoppingCart
                {
                    UserEmail = userEmail,
                    CartItems = new List<ShoppingCartItem>()
                };
            }

            return basket;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        try
        {
            _documentSession.Store(shoppingCart);

            await _documentSession.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            return Errors.Basket.CannotStoreBasket;
        }
    }

    public async Task<Result<bool>> ClearBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            _documentSession.Delete<ShoppingCart>(userEmail);

            await _documentSession.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            return Errors.Basket.CannotStoreBasket;
        }
    }

    public async Task<Result<bool>> DeleteSelectedItemsBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            var basketResult = await GetBasketAsync(userEmail, cancellationToken);

            if (basketResult.IsFailure)
            {
                return basketResult.Error;
            }

            var basket = basketResult.Response!;

            // Remove selected items from the cart
            basket.CartItems.RemoveAll(item => item.IsSelected);

            // Store the updated basket
            _documentSession.Store(basket);
            await _documentSession.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            return Errors.Basket.CannotStoreBasket;
        }
    }
}
