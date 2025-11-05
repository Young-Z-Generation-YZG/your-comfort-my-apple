
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Identity.Domain.Core.Primitives;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users.Entities;

public class Profile : Entity<ProfileId>, IAuditable, ISoftDelete
{
    public Profile(ProfileId id) : base(id)
    {
    }

    private Profile() : base(null!) { }

    required public string FirstName { get; set; }
    required public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    required public DateTime BirthDay { get; set; }
    required public EGender Gender { get; set; } = EGender.OTHER;
    public Image? Image { get; set; } = null;

    required public string UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static Profile Create(ProfileId id,
                                  string firstName,
                                  string lastName,
                                  DateTime birthDay,
                                  EGender gender,
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

    public void Update(string firstName, string lastName, DateTime birthDay, EGender gender)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDay = birthDay;
        Gender = gender;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetGender(string gender)
    {
        EGender.TryFromName(gender, out var genderEnum);

        if (genderEnum is null)
        {
            throw new ArgumentException($"Invalid gender: {gender}");
        }

        Gender = genderEnum;
    }

    public ProfileResponse ToResponse()
    {
        return new ProfileResponse
        {
            Id = Id.Value.ToString() ?? string.Empty,
            UserId = UserId,
            FirstName = FirstName,
            LastName = LastName,
            BirthDay = BirthDay,
            Gender = Gender.ToString(),
            ImageId = Image?.ImageId,
            ImageUrl = Image?.ImageUrl,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }
}
