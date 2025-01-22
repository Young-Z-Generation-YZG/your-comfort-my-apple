
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, bool>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(IIdentityService identityService, ILogger<RegisterCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    //public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    //{
    //    await _identityService.CreateUserAsync(name: request.Name, email: request.Email, password: request.Password);

    //    //using (var scope = _serviceScopeFactory.CreateScope())
    //    //{
    //    //    var identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
    //    //    await identityService.CreateUserAsync(name: request.Name, email: request.Email, password: request.Password);
    //    //}

    //    return true;
    //}
    public async Task<Result<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
