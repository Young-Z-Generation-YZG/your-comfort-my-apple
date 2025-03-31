
using YGZ.Identity.Domain.Core.Abstractions;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users.Events;

public record class UserCreatedDomainEvent(User User) : IDomainEvent
{
    required public string FirstName { get; set; }
    required public string LastName { get; set; }
    required public DateTime BirthDay { get; set; }
    public Gender Gender { get; set; } = Gender.OTHER;
    required public Image? Image { get; set; }
    required public string Country { get; set; }
}