

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Commands.Login;

public sealed record LoginCommand : ICommand<LoginResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
