
using MediatR;
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.Events;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Application.Users.Events;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IIdentityDbContext _dbContext;
    private readonly DbSet<Profile> _profileDbSet;
    private readonly DbSet<ShippingAddress> _shippingAddressDbSet;

    public UserCreatedDomainEventHandler(IIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
        _profileDbSet = dbContext.Profiles;
        _shippingAddressDbSet = dbContext.ShippingAddresses;
    }

    public async Task Handle(UserCreatedDomainEvent request, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);
        try
        {
            var profile = Profile.Create(id: ProfileId.Create(),
                                             firstName: request.FirstName,
                                             lastName: request.LastName,
                                             birthDay: request.BirthDay,
                                             gender: EGender.FromName(request.Gender.Name, false),
                                             image: request.Image,
                                             userId: request.User.Id);

            _profileDbSet.Add(profile);

            var createProfileResult = await _profileDbSet.AddAsync(profile, cancellationToken);

            var shippingAddress = ShippingAddress.Create(id: ShippingAddressId.Create(),
                                                 label: "Default",
                                                 contactName: request.User.Profile.FullName,
                                                 contactPhoneNumber: request.User.PhoneNumber!,
                                                 addressDetail: Address.Create(addressLine: "",
                                                                               addressDistrict: "",
                                                                               addressProvince: "",
                                                                               addressCountry: ""),
                                                 isDefault: true,
                                                 userId: request.User.Id);

            _shippingAddressDbSet.Add(shippingAddress);

            var saveChangesResult = await _dbContext.SaveChangesAsync(cancellationToken);

            if (saveChangesResult > 0)
            {
                await transaction.CommitAsync(cancellationToken);
                return;
            }

            await transaction.RollbackAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }
}
