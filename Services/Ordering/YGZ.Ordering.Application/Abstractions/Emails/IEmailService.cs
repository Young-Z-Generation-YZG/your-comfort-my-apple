using YGZ.Ordering.Application.Emails;

namespace YGZ.Ordering.Application.Abstractions.Emails;

public interface IEmailService
{
    Task SendEmailAsync(EmailCommand mailRequest);
    Task<string> RenderViewToStringAsync(string viewName, object model);
}

