using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using YGZ.Ordering.Application.Emails;
using YGZ.Ordering.Infrastructure.Settings;
using YGZ.Ordering.Application.Abstractions.Emails;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace YGZ.Ordering.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly MailSettings _mailSettings;
    private readonly ILogger<EmailService> _logger;
    private readonly IRazorViewEngine _razorViewEngine;
    private readonly IServiceProvider _serviceProvider;
    private readonly ITempDataProvider _tempDataProvider;

    private readonly string _senderDisplayName;
    private readonly string _senderEmail;
    private readonly string _senderPassword;
    private readonly string _smtpServer;
    private readonly int _smtpPort;

    public EmailService(IOptions<MailSettings> mailSettings,
                       ILogger<EmailService> logger,
                       IRazorViewEngine razorViewEngine,
                       IServiceProvider serviceProvider,
                       ITempDataProvider tempDataProvider)
    {
        _logger = logger;
        _mailSettings = mailSettings.Value;
        _serviceProvider = serviceProvider;
        _razorViewEngine = razorViewEngine;
        _tempDataProvider = tempDataProvider;

        _senderDisplayName = _mailSettings.SenderDisplayName;
        _senderEmail = _mailSettings.SenderEmail;
        _senderPassword = _mailSettings.SenderPassword;
        _smtpServer = _mailSettings.SmtpServer;
        _smtpPort = _mailSettings.SmtpPort;
    }

    public async Task SendEmailAsync(EmailCommand mailRequest)
    {
        var email = new MimeMessage();

        // Sender
        email.From.Add(new MailboxAddress(_senderDisplayName, _senderEmail));

        // Receiver
        email.To.Add(MailboxAddress.Parse(mailRequest.ReceiverEmail));

        // Render the Razor view to get the HTML body
        string htmlBody = await RenderViewToStringAsync(mailRequest.ViewName, mailRequest.Model);

        var builder = new BodyBuilder()
        {
            HtmlBody = htmlBody
        };

        // Handle attachments
        foreach (var attachmentFile in mailRequest.Attachments)
        {
            if (attachmentFile.Length == 0)
            {
                continue;
            }

            using MemoryStream memoryStream = new();
            attachmentFile.CopyTo(memoryStream);
            byte[] attachmentFileByteArray = memoryStream.ToArray();

            builder.Attachments.Add(
                attachmentFile.FileName,
                attachmentFileByteArray,
                ContentType.Parse(attachmentFile.ContentType)
            );
        }

        email.Subject = mailRequest.Subject;
        email.Body = builder.ToMessageBody();

        try
        {
            using SmtpClient smtpClient = new();

            await smtpClient.ConnectAsync(
                _smtpServer,
                _smtpPort,
                SecureSocketOptions.StartTls
            );

            // Authenticate with sender credentials
            await smtpClient.AuthenticateAsync(_senderEmail, _senderPassword);

            await smtpClient.SendAsync(email);

            await smtpClient.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {ReceiverEmail}", mailRequest.ReceiverEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ReceiverEmail}: {ErrorMessage}", mailRequest.ReceiverEmail, ex.Message);
            throw;
        }
    }

    public async Task<string> RenderViewToStringAsync(string viewName, object model)
    {
        var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
        var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

        using var stringWriter = new StringWriter();
        var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

        if (viewResult.View == null)
        {
            throw new ArgumentException($"The view '{viewName}' was not found.");
        }

        var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        {
            Model = model
        };

        var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewData,
            tempData,
            stringWriter,
            new HtmlHelperOptions()
        );

        await viewResult.View.RenderAsync(viewContext);
        return stringWriter.ToString();
    }
}

