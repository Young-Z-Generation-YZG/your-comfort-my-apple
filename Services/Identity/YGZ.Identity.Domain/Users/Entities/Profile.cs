
using YGZ.Identity.Domain.Core.Abstractions;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Core.Primitives;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users.Entities;

public class Profile : Entity<ProfileId>, IAuditable
{
    public Profile(ProfileId id) : base(id)
    {
    }

    private Profile() : base(null!) { }

    required public string FirstName { get; set; }
    required public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    required public DateTime BirthDay { get; set; }
    required public Gender Gender { get; set; } = Gender.OTHER;
    public Image? Image { get; set; } = null;

    required public string UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public static Profile Create(ProfileId id,
                                  string firstName,
                                  string lastName,
                                  DateTime birthDay,
                                  Gender gender,
                                  Image? image,
                                  string userId)
    {
        Profile profile = new Profile(id)
        {
            FirstName = firstName,
            LastName = lastName,
            BirthDay = birthDay,
            Gender = gender,
            Image = image,
            UserId = userId
        };
        return profile;
    }

    public void Update(string firstName, string lastName, DateTime birthDay, Gender gender)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDay = birthDay;
        Gender = gender;
        UpdatedAt = DateTime.UtcNow;
    }
}
