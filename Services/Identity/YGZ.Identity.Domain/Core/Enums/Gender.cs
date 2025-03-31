
using Ardalis.SmartEnum;

namespace YGZ.Identity.Domain.Core.Enums;

public class Gender : SmartEnum<Gender>
{
    public Gender(string name, int value) : base(name, value) { }

    public static readonly Gender MEN = new("MEN", 1);
    public static readonly Gender WOMAN = new("WOMAN", 2);
    public static readonly Gender OTHER = new("OTHER", 3);
}
