

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Keycloak.Commands;

public sealed record AuthorizationCodeCommand : ICommand<TokenResponse>
{
    public required string Code { get; set; }
}
