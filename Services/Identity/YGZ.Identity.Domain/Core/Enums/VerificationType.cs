

using Ardalis.SmartEnum;

namespace YGZ.Identity.Domain.Core.Enums;

public class VerificationType : SmartEnum<VerificationType>
{
    public VerificationType(string name, int value) : base(name, value) { }

    public static readonly VerificationType UNKONW_VERIFICATION = new("UNKONW_VERIFICATION", 0);
    public static readonly VerificationType CREDENTIALS_VERIFICATION = new("CREDENTIALS_VERIFICATION", 1);
    public static readonly VerificationType EMAIL_VERIFICATION = new("EMAIL_VERIFICATION", 2);
    public static readonly VerificationType RESET_PASSWORD_VERIFICATION = new("RESET_PASSWORD_VERIFICATION", 3);
}
