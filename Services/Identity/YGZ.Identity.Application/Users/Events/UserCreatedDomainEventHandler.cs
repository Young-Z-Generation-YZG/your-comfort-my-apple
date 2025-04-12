
using MediatR;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Users.Extensions;
using YGZ.Identity.Domain.Users.Events;

namespace YGZ.Identity.Application.Users.Events;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IAddressRepository _shippingAddressRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;

    public UserCreatedDomainEventHandler(IAddressRepository shippingAddressRepository, IProfileRepository profileRepository, IUserRepository userRepository)
    {
        _shippingAddressRepository = shippingAddressRepository;
        _profileRepository = profileRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(UserCreatedDomainEvent data, CancellationToken cancellationToken)
    {
        var profile = data.ToProfile();

        var createProfileResult = await _profileRepository.AddAsync(profile, cancellationToken);

        if (createProfileResult.IsFailure)
        {
            throw new Exception("Error creating profile");
        }

        var shippingAddress = data.ToShippingAddress();

        var user = await _userRepository.GetDbSet().FindAsync(data.User.Id);

        if(user == null) {
            throw new Exception("User not found");
        }

        user.ShippingAddresses.Add(shippingAddress);

        await _userRepository.SaveChange();
    }
}
