
using YGZ.Identity.Domain.Email.Enums;
using YGZ.Identity.Domain.Email.Models;

namespace YGZ.Identity.Application.Abstractions.Emails;

public interface IEmailClassifier
{
    bool Classified(EmailType type);
    ClassifiedEmail GetEmail(IDictionary<string, string> parameters);
}

