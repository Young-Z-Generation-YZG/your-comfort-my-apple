using YGZ.Identity.Application.Core.Abstractions.Identity;
using YGZ.Identity.Application.Core.Abstractions.Messaging;
using YGZ.Identity.Contracts.Identity;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Identity.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IIdentityService _identityService;
    public CreateUserCommandHandler(
            IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request);

        if(result.IsFailure)
        {
            return result.Error;
        }

        var verificationToken = await _identityService.GenerateEmailVerificationTokenAsync(request.Email);

        if (verificationToken.IsFailure)
        {
            return verificationToken.Error;
        }



        return null;
    }
}
