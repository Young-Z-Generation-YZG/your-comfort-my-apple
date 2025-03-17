

using YGZ.Identity.Application.Emails;

namespace YGZ.Identity.Application.Abstractions.Emails;

public interface IEmailService
{
    Task SendEmailAsync(EmailCommand mailRequest);
    //Task<string> RenderToStringAsync<T>(string viewName, T model) where T : class;
    Task<string> RenderViewToStringAsync(string viewName, object model);

}
