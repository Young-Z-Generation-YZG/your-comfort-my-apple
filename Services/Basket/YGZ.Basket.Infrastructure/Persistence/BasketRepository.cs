

using Marten;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Core.Errors;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Infrastructure.Persistence;

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
                return Errors.Basket.DoesNotExist;
            }

            return basket;
        }
        catch (Exception ex)
        {
            return Errors.Basket.DoesNotExist;
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
