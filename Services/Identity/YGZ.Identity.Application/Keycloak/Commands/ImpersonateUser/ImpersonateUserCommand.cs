using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Keycloak;

namespace YGZ.Identity.Application.Keycloak.Commands.ImpersonateUser;

public sealed record ImpersonateUserCommand : ICommand<TokenExchangeResponse>
{
    public required string UserId { get; init; }
}
