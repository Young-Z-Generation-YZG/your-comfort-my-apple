
using YGZ.Identity.Application.Core.Abstractions.Messaging;
using YGZ.Identity.Contracts.Identity.Login;

namespace YGZ.Identity.Application.Identity.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse> { }
