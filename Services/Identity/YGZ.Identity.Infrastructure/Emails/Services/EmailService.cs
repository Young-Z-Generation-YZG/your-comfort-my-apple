
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using YGZ.Identity.Application.Core.Abstractions.Emails;
using YGZ.Identity.Application.Emails.Commands;
using YGZ.Identity.Infrastructure.Emails.Settings;

namespace YGZ.Identity.Infrastructure.Emails.Services;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<IEmailService> _logger;
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITempDataProvider _tempDataProvider;


    public EmailService(
        IServiceProvider serviceProvider, 
        ILogger<IEmailService> logger, 
        IRazorViewEngine razorViewEngine, 
        ITempDataProvider tempDataProvider,
        IOptions<MailSettings> settings)
    {
        _logger = logger;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
        _razorViewEngine = razorViewEngine;
    }


    public async Task<string> RenderToStringAsync<T>(string viewName, T model) where T : class
    {
        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using var sw = new StringWriter();

        var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

        if(viewResult.View == null)
        {
            throw new ArgumentNullException($"{viewName} does not match any available view");
        }

        var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewDictionary,
            new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
            sw,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);

        return sw.ToString();
    }

    public async Task SendEmailAsync(SendEmailCommand mailRequest)
    {
        var email = new MimeMessage();

        // Sender
        email.From.Add(new MailboxAddress(_mailSettings.SenderDisplayName, _mailSettings.SenderEmail));

        // Receiver
        email.To.Add(MailboxAddress.Parse(mailRequest.EmailTo));

        var MailBuilder = new BodyBuilder()
        {
            HtmlBody = mailRequest.Body
        };

        foreach (var attachmentFile in mailRequest.Attachments)
        {
            if(attachmentFile.Length > 0)
            {
                continue;
            }

            using MemoryStream memoryStream = new();
            attachmentFile.CopyTo(memoryStream);
            byte[] attachmentFileByteArray = memoryStream.ToArray();

            MailBuilder.Attachments.Add(
                attachmentFile.FileName,
                attachmentFileByteArray,
                ContentType.Parse(attachmentFile.ContentType)
            );
        }

        email.Subject = mailRequest.Subject;
        email.Body = MailBuilder.ToMessageBody();

        try
        {
            using SmtpClient smtpClient = new();

            await smtpClient.ConnectAsync(
                _mailSettings.SmtpServer, 
                _mailSettings.SmtpPort, 
                SecureSocketOptions.None
            );

            await smtpClient.SendAsync(email);

            await smtpClient.DisconnectAsync(true);

            _logger.LogInformation("Send email successfully!");

        } catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the email");
        }

        throw new NotImplementedException();
    }
}
