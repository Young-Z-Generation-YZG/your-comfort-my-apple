
using YGZ.Identity.Application.Emails.Commands;

namespace YGZ.Identity.Application.Core.Abstractions.Emails;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailCommand mailRequest);
    Task<string> RenderToStringAsync<T>(string viewName, T model) where T : class;
}
