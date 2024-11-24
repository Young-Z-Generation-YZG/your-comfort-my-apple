
using YGZ.Identity.Application.Emails.Commands;

namespace YGZ.Identity.Application.Core.Abstractions.Emails;

public interface IEmailNotificationService
{
    Task SendEmailConfirmation(EmailConfirmationCommand command);
}
