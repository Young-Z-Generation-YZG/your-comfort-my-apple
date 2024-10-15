
using YGZ.Identity.Application.Emails.Commands;

namespace YGZ.Identity.Application.Core.Abstractions.Emails;

public interface IEmailNotificationService
{
    Task SendConfirmationEmail(EmailConfirmationCommand command);
}
