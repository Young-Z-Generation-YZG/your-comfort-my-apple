using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using YGZ.IdentityServer.Infrastructure.Settings;

namespace YGZ.IdentityServer.Infrastructure.Mails;

public class SendMailService : IEmailSender
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<SendMailService> _logger;

    public SendMailService(IOptions<MailSettings> mailSettings, ILogger<SendMailService> logger)
    {
        _mailSettings = mailSettings.Value;
        _logger = logger;
    }


    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var emailBuilder = new MimeMessage();

        emailBuilder.Sender = new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail);

        emailBuilder.From.Add(new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail));

        emailBuilder.To.Add(MailboxAddress.Parse(email));

        emailBuilder.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = htmlMessage;
        emailBuilder.Body = bodyBuilder.ToMessageBody();

        using SmtpClient smtpClient = new();

        try
        {
            smtpClient.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.SmtpPassword);

            await smtpClient.SendAsync(emailBuilder);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Send email error! {ex.Message}");
        }

        smtpClient.Disconnect(true);

        _logger.LogInformation("send mail to: " + email);
    }
}
