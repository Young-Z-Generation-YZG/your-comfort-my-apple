

using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Promotions.ValueObjects;

namespace YGZ.Catalog.Domain.Promotions;

public class Promotion : Entity<PromotionId>, IAuditable
{
    public string Name { get; }

    public DateTime Created_at { get; }

    public DateTime Updated_at { get; set; }

    private Promotion(PromotionId id, string name, DateTime created_at, DateTime updated_at) : base(id)
    {
        Name = name;
        Created_at = created_at;
        Updated_at = updated_at;
    }

    public static Promotion Create(string name)
    {
        var vietnamTimezone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimezone);

        return new(PromotionId.Create(), name, vietnamTime, vietnamTime);
    }
}
