using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Auths.Commands.Login;

namespace YGZ.Identity.Application.Tests;

public class TestLoggingHandler : ICommandHandler<TestLoggingCommand, bool>
{
    private readonly ILogger<LoginHandler> _logger;

    public TestLoggingHandler(ILogger<LoginHandler> logger)
    {
        _logger = logger;
    }

    public Task<Result<bool>> Handle(TestLoggingCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
