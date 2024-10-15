using YGZ.Identity.Application.Common.Abstractions.Messaging;
using YGZ.Identity.Contracts.Identity;

namespace YGZ.Identity.Application.Identity.Commands.CreateUser;

public sealed record CreateUserCommand(string FirstName, string LastName, string Email, string Password) : ICommand<CreateUserResponse> { }