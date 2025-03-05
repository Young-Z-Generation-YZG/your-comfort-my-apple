

using YGZ.Keycloak.Domain.Email.Enums;
using YGZ.Keycloak.Domain.Email.Models;

namespace YGZ.Keycloak.Application.Abstractions.Emails;

public interface IEmailClassifier
{
    bool Classified(EmailType type);
    ClassifiedEmail GetEmail(IDictionary<string, string> parameters);
}

