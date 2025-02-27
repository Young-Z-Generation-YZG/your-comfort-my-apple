using YGZ.Keycloak.Application.Abstractions.Mails;
using YGZ.Keycloak.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace YGZ.Keycloak.Infrastructure.Mail;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<MailService> _logger;
    private readonly string _emailHost;
    private readonly int _emailPort;
    private readonly string _emailName;
    private readonly string _emailPassword;

    public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;

        _emailHost = _mailSettings.Host;
        _emailPort = _mailSettings.Port;
        _emailName = _mailSettings.Email;
        _emailPassword = _mailSettings.Passowrd;
    }

    public async Task<bool> SendMailAsync(string receptor, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(_emailHost, _emailPort);

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential(_emailName, _emailPassword);

            var message = new MailMessage(_emailName, receptor, subject, body);

            await smtpClient.SendMailAsync(message);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return false;
        }
    }
}
