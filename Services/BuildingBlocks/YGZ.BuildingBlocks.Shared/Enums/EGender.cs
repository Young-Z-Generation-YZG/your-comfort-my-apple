using Ardalis.SmartEnum;

namespace YGZ.BuildingBlocks.Shared.Enums;

public class EGender : SmartEnum<EGender>
{
    public EGender(string name, int value) : base(name, value) { }

    public static readonly EGender UNKNOWN = new(nameof(UNKNOWN), 0);
    public static readonly EGender MALE = new(nameof(MALE), 0);
    public static readonly EGender FEMALE = new(nameof(FEMALE), 0);
    public static readonly EGender OTHER = new(nameof(OTHER), 0);
}
