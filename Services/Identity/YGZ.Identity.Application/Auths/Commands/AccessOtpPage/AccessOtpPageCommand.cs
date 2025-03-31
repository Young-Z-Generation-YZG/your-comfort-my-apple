

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Auths.Commands.AccessOtpPage;

public sealed record AccessOtpPageCommand(string Email, string Token, string VerifyType) : ICommand<bool> { }
