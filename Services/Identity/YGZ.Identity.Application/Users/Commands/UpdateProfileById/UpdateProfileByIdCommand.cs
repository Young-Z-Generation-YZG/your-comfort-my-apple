using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfileById;

public sealed record UpdateProfileByIdCommand : ICommand<bool>
{
    public required string UserId { get; set; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? BirthDay { get; init; }
    public string? Gender { get; init; }
}
