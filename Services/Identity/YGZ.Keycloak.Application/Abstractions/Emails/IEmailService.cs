

using YGZ.Keycloak.Application.Emails;

namespace YGZ.Keycloak.Application.Abstractions.Mails;

public interface IEmailService
{
    Task SendEmailAsync(EmailCommand mailRequest);
    //Task<string> RenderToStringAsync<T>(string viewName, T model) where T : class;
    Task<string> RenderViewToStringAsync(string viewName, object model);

}
