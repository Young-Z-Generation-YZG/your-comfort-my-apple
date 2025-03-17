
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Emails;

public sealed record TestSendEmailCommand(
    string ReceiverEmail,
    string Subject,
    string Body) : ICommand<bool>
{
}