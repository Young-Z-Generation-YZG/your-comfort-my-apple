using Microsoft.AspNetCore.Identity;
using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Domain.Core.Abstractions;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.Events;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users;

public class User : IdentityUser, IAggregate, IAuditable, ISoftDelete
{
    public string? TenantId { get; set; }
    public string? BranchId { get; set; }
    public string? TenantCode { get; set; }
    public Profile Profile { get; set; } = null!; // One-to-one relationship
    public ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>(); // One-to-many relationship
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;


    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();



    public static User Create(Guid guid,
                              string email,
                              string phoneNumber,
                              string passwordHash,
                              string firstName,
                              string lastName,
                              DateTime birthDay,
                              Image? image,
                              string country,
                              bool? emailConfirmed,
                              string? tenantId,
                              string? branchId,
                              string? tenantCode)
    {

        var user = new User
        {
            Id = guid.ToString(),
            Email = email,
            UserName = email,
            NormalizedEmail = email.ToUpper(),
            NormalizedUserName = email.ToUpper(),
            PasswordHash = passwordHash,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = false,
            EmailConfirmed = emailConfirmed ?? false,
            TenantId = tenantId,
            BranchId = branchId,
            TenantCode = tenantCode,
        };

        user.AddDomainEvent(new UserCreatedDomainEvent(user)
        {
            FirstName = firstName,
            LastName = lastName,
            BirthDay = birthDay,
            Image = image,
            Country = country
        });

        return user;
    }

    public void Update(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        if (user.PhoneNumber != null)
            PhoneNumber = user.PhoneNumber;
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeueEvents = _domainEvents.ToArray();

        _domainEvents.Clear();

        return dequeueEvents;
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public GetAccountResponse ToAccountResponse()
    {
        return new GetAccountResponse
        {
            Email = Email!,
            FirstName = Profile.FirstName,
            LastName = Profile.LastName,
            PhoneNumber = PhoneNumber!,
            BirthDate = Profile.BirthDay.ToString("yyyy-MM-dd"),
            ImageId = Profile.Image!.ImageId,
            ImageUrl = Profile.Image.ImageUrl,
            DefaultAddressLabel = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.Label ?? "",
            DefaultContactName = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.ContactName ?? "",
            DefaultContactPhoneNumber = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.ContactPhoneNumber ?? "",
            DefaultAddressLine = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.AddressDetail.AddressLine ?? "",
            DefaultAddressDistrict = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.AddressDetail.AddressDistrict ?? "",
            DefaultAddressProvince = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.AddressDetail.AddressProvince ?? "",
            DefaultAddressCountry = ShippingAddresses.FirstOrDefault(x => x.IsDefault)?.AddressDetail.AddressCountry ?? "",
        };
    }

    public UserResponse ToResponse()
    {
        return new UserResponse
        {
            Id = Id,
            TenantId = TenantId,
            BranchId = BranchId,
            TenantCode = TenantCode,
            UserName = UserName ?? string.Empty,
            NormalizedUserName = NormalizedUserName ?? string.Empty,
            Email = Email!,
            NormalizedEmail = NormalizedEmail ?? string.Empty,
            EmailConfirmed = EmailConfirmed,
            PhoneNumber = PhoneNumber!,
            Profile = Profile.ToResponse(),
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy,
        };
    }
}
