

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Application.Auths.Commands.AccessOtpPage;

public class AccessOtpPageCommandHandler : ICommandHandler<AccessOtpPageCommand, bool>
{
    private readonly IIdentityService _identityService;

    public AccessOtpPageCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<bool>> Handle(AccessOtpPageCommand request, CancellationToken cancellationToken)
    {
        //var result = await _identityService.CheckTokenIsValid(request.Email, request.Token);

        return true;
    }
}
