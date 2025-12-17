using Microsoft.AspNetCore.Http;

namespace YGZ.Ordering.Application.Emails;

public sealed record EmailCommand(
    string ReceiverEmail,
    string Subject,
    string ViewName,
    object Model,
    IEnumerable<IFormFile>? Attachments = default
)
{
    public IEnumerable<IFormFile> Attachments { get; init; } = Attachments ?? Enumerable.Empty<IFormFile>();
};

