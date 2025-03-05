

namespace YGZ.Keycloak.Infrastructure.Settings;

public class MailSettings
{
    public const string SettingKey = "MailSettings";

    /// <summary>
    /// Gets or sets the SMTP server.
    /// </summary>
    public string SmtpServer { get; set; } = default!;

    /// <summary>
    /// Gets or sets the SMTP port.
    /// </summary>
    public int SmtpPort { get; set; }
    /// <summary>
    /// Gets or sets the email sender display name.
    /// </summary>
    public string SenderDisplayName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the email sender.
    /// </summary>
    public string SenderEmail { get; set; } = default!;

    /// <summary>
    /// Gets or sets the SMTP password.
    /// </summary>
    public string SenderPassword { get; set; } = default!;
}

