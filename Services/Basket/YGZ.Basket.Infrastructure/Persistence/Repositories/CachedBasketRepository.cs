
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.ShoppingCart;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using Microsoft.Extensions.Logging;

namespace YGZ.Basket.Infrastructure.Persistence.Repositories;

public class CachedBasketRepository : IBasketRepository
{
    private readonly ILogger<CachedBasketRepository> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IDistributedCache _distributedCache;

    public CachedBasketRepository( ILogger<CachedBasketRepository> logger, IBasketRepository basketRepository, IDistributedCache distributedCache)
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _distributedCache = distributedCache;
    }

    public async Task<Result<ShoppingCart>> GetBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try {
        var cachedBasket = await _distributedCache.GetStringAsync(userEmail, cancellationToken);

        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);

        await _distributedCache.SetStringAsync(userEmail, JsonSerializer.Serialize(basket.Response), cancellationToken);

        return basket;
        } catch (Exception ex) {
            _logger.LogError("Failed to get basket: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<Result<bool>> StoreBasketAsync(ShoppingCart shoppingCart, CancellationToken cancellationToken)
    {
        try {
        await _basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

        await _distributedCache.SetStringAsync(shoppingCart.UserEmail, JsonSerializer.Serialize(shoppingCart), cancellationToken);

        return true;
        } catch (Exception ex) {
            _logger.LogError("Failed to store basket: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<Result<bool>> DeleteSelectedItemsBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _basketRepository.DeleteSelectedItemsBasketAsync(userEmail, cancellationToken);

            if (result.IsFailure)
            {
                return result;
            }

            // Get the updated basket and update cache
            var basketResult = await _basketRepository.GetBasketAsync(userEmail, cancellationToken);
            if (basketResult.IsSuccess)
            {
                await _distributedCache.SetStringAsync(userEmail, JsonSerializer.Serialize(basketResult.Response), cancellationToken);
            }

            return result;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Result<bool>> ClearBasketAsync(string userEmail, CancellationToken cancellationToken)
    {
        try
        {
            await _basketRepository.ClearBasketAsync(userEmail, cancellationToken);

            await _distributedCache.RemoveAsync(userEmail, cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;


    }
}
