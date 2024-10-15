using YGZ.Identity.Application.Common.Abstractions.Messaging;
using YGZ.Identity.Application.Identity.Common.Abstractions;
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
        throw new NotImplementedException();
    }
}
