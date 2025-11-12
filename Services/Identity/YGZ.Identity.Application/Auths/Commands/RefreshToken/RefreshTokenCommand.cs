
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Commands.RefreshToken;

public sealed record RefreshTokenCommand : ICommand<TokenResponse>
{
    required public string RefreshToken { get; init; }
}
