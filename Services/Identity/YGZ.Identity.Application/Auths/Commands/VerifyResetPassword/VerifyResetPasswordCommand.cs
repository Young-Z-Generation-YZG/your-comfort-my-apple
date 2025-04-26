

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.VerifyResetPassword;

public sealed record VerifyResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmPassword) : ICommand<bool> { }