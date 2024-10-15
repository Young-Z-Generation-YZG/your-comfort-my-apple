
using YGZ.Identity.Application.Core.Abstractions.Emails;
using YGZ.Identity.Domain.Emails.Constants;
using YGZ.Identity.Domain.Emails.Enums;
using YGZ.Identity.Domain.Emails.Models;
namespace YGZ.Identity.Infrastructure.Emails.Templates;

public class ResetPasswordTemplate : IEmailClassifier
{
    public bool Classified(EEmailType type) => EEmailType.ResetPassword == type;

    public ClassifiedEmail GetEmail(IDictionary<string, string> bodyParameters)
    {
        ClassifiedEmail classified =
            new()
            {
                Subject = EmailSubject.PasswordReset,
                Body = EmailBody.PasswordReset(bodyParameters["fullName"], bodyParameters["url"])
            };

        return classified;
    }
}
