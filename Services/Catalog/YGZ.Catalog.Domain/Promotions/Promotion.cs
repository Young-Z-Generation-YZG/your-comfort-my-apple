

using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Promotions.ValueObjects;

namespace YGZ.Catalog.Domain.Promotions;

public class Promotion : Entity<PromotionId>, IAuditable
{
    public string Name { get; }

    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; set; }

    private Promotion(PromotionId id, string name, DateTime createdAt, DateTime updatedAt) : base(id)
    {
        Name = name;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Promotion Create(string name)
    {
        var vietnamTimezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimezone);

        return new(PromotionId.CreateNew(), name, vietnamTime, vietnamTime);
    }
}
