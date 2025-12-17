namespace YGZ.Ordering.Infrastructure.Settings;

public class MailSettings
{
    public const string SettingKey = "MailSettings";

    public string SmtpServer { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SenderDisplayName { get; set; } = default!;
    public string SenderEmail { get; set; } = default!;
    public string SenderPassword { get; set; } = default!;
}

