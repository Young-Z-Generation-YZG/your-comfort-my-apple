
using Ardalis.SmartEnum;

namespace YGZ.Identity.Domain.Core.Enums;

public class Gender : SmartEnum<Gender>
{
    public Gender(string name, int value) : base(name, value) { }

    public static readonly Gender UNKOWN = new("UNKOWN", 0);
    public static readonly Gender MALE = new("MALE", 1);
    public static readonly Gender FEMALE = new("FEMALE", 2);
    public static readonly Gender OTHER = new("OTHER", 3);
}
