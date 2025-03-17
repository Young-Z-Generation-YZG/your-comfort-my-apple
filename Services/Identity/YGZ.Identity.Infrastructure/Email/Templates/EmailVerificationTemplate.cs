

using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Domain.Email.Constants;
using YGZ.Identity.Domain.Email.Enums;
using YGZ.Identity.Domain.Email.Models;

namespace YGZ.Identity.Infrastructure.Email.Templates;

public class EmailVerificationTemplate : IEmailClassifier
{
    public bool Classified(EmailType type) => EmailType.Verification == type;

    public ClassifiedEmail GetEmail(IDictionary<string, string> parameters)
    {
        ClassifiedEmail classified =
            new()
            {
                Subject = EmailSubject.Verification,
                Body = EmailBody.Verification(parameters["fullName"], parameters["url"])
            };

        return classified;
    }
}
