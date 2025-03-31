
using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Domain.Users.ValueObjects;

public class ProfileId : ValueObject
{
    public Guid? Value { get; set; } = null;

    private ProfileId() { }
    private ProfileId(Guid? guid) => Value = guid;

    public static ProfileId Create()
    {
        return new ProfileId(Guid.NewGuid());
    }

    public static ProfileId Of(Guid? guid)
    {
        return new ProfileId(guid);
    }

    public static ProfileId Of(string guid)
    {
        var isParsed = Guid.TryParse(guid, out Guid result);

        return new ProfileId(result);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
