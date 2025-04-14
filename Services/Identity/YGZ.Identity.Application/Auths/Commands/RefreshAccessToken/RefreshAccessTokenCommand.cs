
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Commands.RefreshAccessToken;

public sealed record RefreshAccessTokenCommand : ICommand<RefreshAccessTokenResponse>
{
    required public string RefreshToken { get; init; }
}
