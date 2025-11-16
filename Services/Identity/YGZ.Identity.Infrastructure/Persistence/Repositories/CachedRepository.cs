
using Microsoft.Extensions.Caching.Distributed;
using YGZ.Identity.Application.Abstractions.Data;
using Microsoft.Extensions.Logging;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class CachedRepository : ICachedRepository
{
    private readonly ILogger<CachedRepository> _logger;
    private readonly IDistributedCache _distributedCache;

    public CachedRepository(IDistributedCache distributedCache, ILogger<CachedRepository> logger)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }

    public async Task SetAsync(string key, string value, TimeSpan? ttl = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (ttl.HasValue)
        {
            options.SetAbsoluteExpiration(ttl.Value);
        } else
        {
            options.SetSlidingExpiration(TimeSpan.FromMinutes(1));
        }

        try {
            await _distributedCache.SetStringAsync(key, value, options);
        } catch (Exception ex) {
            _logger.LogError("Failed to set cached repository: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<string?> GetAsync(string key)
    {
        try {
            return await _distributedCache.GetStringAsync(key);
        } catch (Exception ex) {
            _logger.LogError("Failed to get cached repository: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task RemoveAsync(string key)
    {
        try {
            await _distributedCache.RemoveAsync(key);
        } catch (Exception ex) {
            _logger.LogError("Failed to remove cached repository: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
