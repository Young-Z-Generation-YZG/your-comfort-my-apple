using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Seeds;

public static class SeedEventData
{
    public static IEnumerable<Event> Events
    {
        get
        {
            return new List<Event>
            {
                Event.Create(
                    id: EventId.Of("611db6eb-3d64-474e-9e23-3517ad0df6ec"),
                    title: "Black Friday",
                    description: "Sale all item in shop with special price",
                    startDate: DateTime.UtcNow,
                    endDate: DateTime.UtcNow.AddDays(10)
                ),
            };
        }
    }
}
