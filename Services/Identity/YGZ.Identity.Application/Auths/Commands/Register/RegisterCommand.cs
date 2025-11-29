
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public sealed record RegisterCommand : ICommand<EmailVerificationResponse>
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
    public required string PhoneNumber { get; init; }
    public required string BirthDate { get; init; }
    public required string Country { get; init; }
}