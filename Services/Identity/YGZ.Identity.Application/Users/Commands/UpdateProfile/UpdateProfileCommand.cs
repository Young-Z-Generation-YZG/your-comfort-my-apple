using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfile;

public sealed record UpdateProfileCommand() : ICommand<bool>
{
    public required string ProfileId { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string BirthDay { get; init; }
    public required string Gender { get; init; }
}
