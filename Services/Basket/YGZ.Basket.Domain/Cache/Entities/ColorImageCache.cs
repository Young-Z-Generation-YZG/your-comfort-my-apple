using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Domain.Cache.Entities;

public class ColorImageCache
{
    public required string ModelId { get; init; }
    public required Color Color { get; init; }
    public required string DisplayImageUrl { get; init; }

    public static ColorImageCache Create(string modelId,
                                  Color color,
                                  string displayImageUrl)
    {
        return new ColorImageCache
        {
            ModelId = modelId,
            Color = color,
            DisplayImageUrl = displayImageUrl
        };
    }

    public static ColorImageCache Of(string modelId, Color color)
    {
        return new ColorImageCache
        {
            ModelId = modelId,
            Color = color,
            DisplayImageUrl = string.Empty
        };
    }
    public string GetCachedKey() => $"IMAGE_{ModelId}:{Color.NormalizedName}";
}
