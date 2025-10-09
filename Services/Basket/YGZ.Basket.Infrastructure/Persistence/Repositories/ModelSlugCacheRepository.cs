using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.Basket.Application.Abstractions.Data;
using YGZ.Basket.Domain.Cache.Entities;

namespace YGZ.Basket.Infrastructure.Persistence.Repositories;

public class ModelSlugCacheRepository : IModelSlugCache
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<ModelSlugCacheRepository> _logger;

    public ModelSlugCacheRepository(IDistributedCache distributedCache, ILogger<ModelSlugCacheRepository> logger)
    {
        _logger = logger;
        _distributedCache = distributedCache;
    }

    private static readonly DistributedCacheEntryOptions _cacheOptions = new();

    public async Task SetSlugAsync(ModelSlugCache modelSlug, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = modelSlug.GetCachedKey();
            var value = modelSlug.ModelSlug;

            await _distributedCache.SetStringAsync(cacheKey, value, _cacheOptions, cancellationToken);

            _logger.LogInformation("Model slug cached for modelId: {ModelId}, Slug: {Slug}",
                modelSlug.ModelId, value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching model slug for modelId: {ModelId}",
                modelSlug.ModelId);

            throw;
        }
    }

    public async Task SetSlugsBatchAsync(IEnumerable<ModelSlugCache> modelSlugs, CancellationToken cancellationToken = default)
    {
        try
        {
            var tasks = modelSlugs.Select(slug => SetSlugAsync(slug, cancellationToken));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Batch cached {Count} model slugs", modelSlugs.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during batch model slug caching");

            throw;
        }
    }

    public async Task<string?> GetSlugAsync(ModelSlugCache modelSlug, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = modelSlug.GetCachedKey();
            var value = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            return value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting model slug for modelId: {ModelId}",
                modelSlug.ModelId);

            throw;
        }
    }

    public async Task<bool> ExistsAsync(ModelSlugCache modelSlug, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = modelSlug.GetCachedKey();
            var value = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            return value is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if model slug exists for modelId: {ModelId}",
                modelSlug.ModelId);

            throw;
        }
    }
}
