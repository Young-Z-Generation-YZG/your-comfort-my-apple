

using YGZ.Keycloak.Application.Abstractions.Emails;
using YGZ.Keycloak.Domain.Email.Constants;
using YGZ.Keycloak.Domain.Email.Enums;
using YGZ.Keycloak.Domain.Email.Models;

namespace YGZ.Keycloak.Infrastructure.Mail.Templates;

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
