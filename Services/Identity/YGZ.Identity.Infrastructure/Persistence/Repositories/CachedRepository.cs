
using Microsoft.Extensions.Caching.Distributed;
using YGZ.Identity.Application.Abstractions.Data;

namespace YGZ.Identity.Infrastructure.Persistence.Repositories;

public class CachedRepository : ICachedRepository
{
    private readonly IDistributedCache _distributedCache;

    public CachedRepository(IDistributedCache distributedCache)
    {
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

        await _distributedCache.SetStringAsync(key, value);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _distributedCache.GetStringAsync(key);
    }

    public async Task RemoveAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
