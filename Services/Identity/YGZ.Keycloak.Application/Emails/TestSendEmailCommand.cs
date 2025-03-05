

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Keycloak.Application.Emails;

public sealed record TestSendEmailCommand(
    string ReceiverEmail,
    string Subject,
    string Body) : ICommand<bool>
{
}