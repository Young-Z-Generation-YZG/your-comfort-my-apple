
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.ChangePassword;

public sealed record ChangePasswordCommand : ICommand<bool>
{
    required public string OldPassword { get; init; }
    required public string NewPassword { get; init; }
}
