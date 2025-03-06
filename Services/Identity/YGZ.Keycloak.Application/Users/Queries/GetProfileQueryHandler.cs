
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Keycloak.Application.Abstractions;

namespace YGZ.Keycloak.Application.Users.Queries;

public class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, string>
{
    private readonly IUserContext _userContext;
    private readonly ILogger<GetProfileQueryHandler> _logger;

    public GetProfileQueryHandler(IUserContext userContext, ILogger<GetProfileQueryHandler> logger)
    {
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var email = _userContext.GetUserEmail();

        _logger.LogInformation("User {email} requested profile", email);

        return email;
    }
}
