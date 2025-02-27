

namespace YGZ.Keycloak.Application.Abstractions.Mails;

public interface IMailService
{
    Task<bool> SendMailAsync(string receptor, string subject, string body);
}
