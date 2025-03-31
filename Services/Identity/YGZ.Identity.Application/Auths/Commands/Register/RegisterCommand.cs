
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public sealed record RegisterCommand : ICommand<EmailVerificationResponse>
{
    required public string FirstName { get; set; }
    required public string LastName { get; set; }
    required public string Email { get; set; }
    required public string Password { get; set; }
    required public string ConfirmPassword { get; set; }
    required public string PhoneNumber { get; set; }
    required public string BirthDay { get; set; }
    required public string Country { get; set; }
}