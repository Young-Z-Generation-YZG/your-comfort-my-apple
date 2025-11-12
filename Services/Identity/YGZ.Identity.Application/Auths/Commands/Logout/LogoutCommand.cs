using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.Logout;

public sealed record LogoutCommand : ICommand<bool>
{
    public required string RefreshToken { get; init; }
}
