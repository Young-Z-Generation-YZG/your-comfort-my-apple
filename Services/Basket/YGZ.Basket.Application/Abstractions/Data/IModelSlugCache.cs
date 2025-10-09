using YGZ.Basket.Domain.Cache.Entities;

namespace YGZ.Basket.Application.Abstractions.Data;

public interface IModelSlugCache
{
    Task<string?> GetSlugAsync(ModelSlugCache modelSlug, CancellationToken cancellationToken = default);
    Task SetSlugAsync(ModelSlugCache modelSlug, CancellationToken cancellationToken = default);
    Task SetSlugsBatchAsync(IEnumerable<ModelSlugCache> modelSlugs, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(ModelSlugCache modelSlug, CancellationToken cancellationToken = default);
}
