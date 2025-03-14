

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
                    Items = new List<ShoppingCartItem>()
                };
            }

            return basket;
        }
        catch (Exception ex)
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
        catch (Exception ex)
        {
            return Errors.Basket.CannotStoreBasket;
        }
    }

    public async Task<Result<bool>> DeleteBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            _documentSession.Delete<ShoppingCart>(userEmail);

            await _documentSession.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            return Errors.Basket.CannotStoreBasket;
        }
    }

}
