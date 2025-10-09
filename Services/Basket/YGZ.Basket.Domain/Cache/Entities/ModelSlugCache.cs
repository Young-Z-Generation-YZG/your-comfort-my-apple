namespace YGZ.Basket.Domain.Cache.Entities;

public class ModelSlugCache
{
    public required string ModelId { get; init; }
    public required string ModelSlug { get; init; }

    public static ModelSlugCache Create(string modelId,
                                  string modelSlug)
    {
        return new ModelSlugCache
        {
            ModelId = modelId,
            ModelSlug = modelSlug
        };
    }

    public static ModelSlugCache Of(string modelId)
    {
        return new ModelSlugCache
        {
            ModelId = modelId,
            ModelSlug = string.Empty
        };
    }
    public string GetCachedKey() => $"SLUG_{ModelId}";
}
