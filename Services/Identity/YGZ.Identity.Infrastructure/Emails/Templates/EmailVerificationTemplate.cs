
using YGZ.Identity.Application.Core.Abstractions.Emails;
using YGZ.Identity.Domain.Emails.Constants;
using YGZ.Identity.Domain.Emails.Enums;
using YGZ.Identity.Domain.Emails.Models;

namespace YGZ.Identity.Infrastructure.Emails.Templates;

public class EmailVerificationTemplate : IEmailClassifier
{
    public bool Classified(EEmailType type) => EEmailType.Verification == type;

    public ClassifiedEmail GetEmail(IDictionary<string, string> parameters)
    {
        ClassifiedEmail classified = new()
        {
            Subject = EmailSubject.EmailVerification,
            Body = EmailBody.Verification(parameters["fullName"], parameters["url"])
        };

        return classified;
    }
}
