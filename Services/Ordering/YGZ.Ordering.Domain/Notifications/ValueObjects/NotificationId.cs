using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Notifications.ValueObjects;

public class NotificationId : ValueObject
{
    public Guid Value { get; init; }

    private NotificationId(Guid guid)
    {
        Value = guid;
    }

    public static NotificationId Create()
    {
        return new NotificationId(Guid.NewGuid());
    }

    public static NotificationId Of(string guid)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(guid);

        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new NotificationId(parsedGuid);
    }

    public static NotificationId Of(Guid guid)
    {
        return new NotificationId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
