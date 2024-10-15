
using YGZ.Identity.Domain.Emails.Enums;
using YGZ.Identity.Domain.Emails.Models;

namespace YGZ.Identity.Application.Core.Abstractions.Emails;

public interface IEmailClassifier
{
    bool Classified(EEmailType type);
    ClassifiedEmail GetEmail(IDictionary<string, string> parameters);
}
