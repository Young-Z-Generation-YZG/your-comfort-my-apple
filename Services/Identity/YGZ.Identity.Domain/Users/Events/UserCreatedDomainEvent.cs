
using YGZ.Identity.Domain.Core.Abstractions;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users.Events;

public record class UserCreatedDomainEvent(User User) : IDomainEvent
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime BirthDay { get; set; }
    public Gender Gender { get; set; } = Gender.OTHER;
    public required Image? Image { get; set; }
    public required string Country { get; set; }
}