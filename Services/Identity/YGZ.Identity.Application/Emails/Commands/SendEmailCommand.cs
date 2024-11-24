using Microsoft.AspNetCore.Http;

namespace YGZ.Identity.Application.Emails.Commands;

public sealed record SendEmailCommand(string EmailTo, string Subject, string Body, IEnumerable<IFormFile> Attachments = default!)
{
    public IEnumerable<IFormFile> Attachments { get; init; } = Attachments ?? Enumerable.Empty<IFormFile>();
}

