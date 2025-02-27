

namespace YGZ.Keycloak.Infrastructure.Settings;

public class MailSettings
{
    public const string SettingKey = "MailSettings";

    public string Host { get; set; } = default!;
    public int Port { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Passowrd { get; set; } = default!;


    //public string EmailId { get; set; }
    //public string Name { get; set; }
    //public string UserName { get; set; }
    //public string Password { get; set; }
    //public string Host { get; set; }
    //public int Port { get; set; }
    //public bool UseSSL { get; set; }

    ///// <summary>
    ///// Gets or sets the SMTP server.
    ///// </summary>
    //public string Server { get; set; } = default!;

    ///// <summary>
    ///// Gets or sets the SMTP port.
    ///// </summary>
    //public int Port { get; set; }
    ///// <summary>
    ///// Gets or sets the email sender display name.
    ///// </summary>
    //public string SenderName { get; set; } = default!;

    ///// <summary>
    ///// Gets or sets the email sender.
    ///// </summary>
    //public string SenderEmail { get; set; } = default!;

    ///// <summary>
    ///// Gets or sets the SMTP password.
    ///// </summary>
    //public string Password { get; set; } = default!;
}

