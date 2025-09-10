
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Application.Auths.Commands.TestLogin;

public class TestLoginCommandHandler : ICommandHandler<TestLoginCommand, LoginResponse>
{
  private readonly ILogger<TestLoginCommandHandler> _logger;

  public TestLoginCommandHandler(ILogger<TestLoginCommandHandler> logger)
  {
    _logger = logger;
  }

  public async Task<Result<LoginResponse>> Handle(TestLoginCommand request, CancellationToken cancellationToken)
  {
    return Errors.User.DoesNotExist;

    throw new NotImplementedException();
  }
}