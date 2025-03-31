

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.VerifyEmail;

public sealed record VerifyEmailCommand(string Email, string Token, string Otp) : ICommand<bool>;
