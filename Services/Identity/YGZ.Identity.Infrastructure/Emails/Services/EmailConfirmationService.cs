
using YGZ.Identity.Application.Core.Abstractions.Emails;
using YGZ.Identity.Application.Emails.Commands;
using YGZ.Identity.Domain.Emails.Constants;

namespace YGZ.Identity.Infrastructure.Emails.Services;

public class EmailConfirmationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;

    public EmailConfirmationService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task SendConfirmationEmail(EmailConfirmationCommand command)
    {
        var mail = 
            new SendEmailCommand(
                EmailTo: command.Email,
                Subject: EmailSubject.EmailConfirmation,
                await _emailService.RenderToStringAsync(EmailConstants.Verification, command)
            );

        await _emailService.SendEmailAsync(mail);
    }
}
