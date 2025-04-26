

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Commands.ResetPassword;

public sealed record ResetPasswordCommand : ICommand<ResetPasswordVerificationResponse>
{
    public required string Email { get; set; }
}
