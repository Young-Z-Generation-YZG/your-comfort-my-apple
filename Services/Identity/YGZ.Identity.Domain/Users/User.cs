﻿using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Core.Abstractions;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.Events;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users;

public class User : IdentityUser, IAggregate
{
    public Profile Profile { get; set; } = null!; // One-to-one relationship
    public ProfileId? ProfileId { get; set; }
    public ICollection<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>(); // One-to-many relationship

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
                              string country
        )
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
}
