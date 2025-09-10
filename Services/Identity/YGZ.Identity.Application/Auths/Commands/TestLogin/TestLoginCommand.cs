
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Commands.TestLogin;
public sealed record TestLoginCommand : ICommand<LoginResponse>
{
  public string Email { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}
