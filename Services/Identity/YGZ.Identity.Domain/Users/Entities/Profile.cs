
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

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string FullName { get; private set; } = string.Empty;
    public required string PhoneNumber { get; set; }
    public required DateTime BirthDay { get; set; }
    public required EGender Gender { get; set; } = EGender.OTHER;
    public Image? Image { get; set; } = null;

    public required string UserId { get; set; }
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
                                  string phoneNumber,
                                  DateTime birthDay,
                                  EGender gender,
                                  Image? image,
                                  string userId)
    {
        Profile profile = new Profile(id)
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            BirthDay = birthDay,
            Gender = gender,
            Image = image,
            UserId = userId
        };
        profile.SetFullName();
        return profile;
    }

    public void Update(string? firstName, string? lastName, string? phoneNumber, DateTime? birthDay, EGender? gender)
    {
        FirstName = firstName ?? FirstName;
        LastName = lastName ?? LastName;
        PhoneNumber = phoneNumber ?? PhoneNumber;
        BirthDay = birthDay ?? BirthDay;
        Gender = gender ?? Gender;
        UpdatedAt = DateTime.UtcNow;
        SetFullName();
    }

    private void SetFullName()
    {
        FullName = $"{FirstName} {LastName}";
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
            FullName = FullName,
            PhoneNumber = PhoneNumber,
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
