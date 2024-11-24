using YGZ.Identity.Application.Core.Abstractions.Messaging;

namespace YGZ.Identity.Application.Identity.Commands.CreateUser;

public sealed record CreateUserCommand(string FirstName, string LastName, string Email, string Password) : ICommand<bool> { }