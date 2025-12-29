

using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.BuildingBlocks.Shared.Behaviors;

public class RequestLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : class 
    where TResponse : Result
{
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingPipelineBehavior(ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;

        _logger.LogInformation("[RequestLoggingPipelineBehavior]:: Processing {RequestName}", requestName);

        TResponse result = await next();

        if (result.IsSuccess)
        {
            _logger.LogInformation("[RequestLoggingPipelineBehavior]:: Completed {RequestName}", requestName);
        }
        else
        {
            using (LogContext.PushProperty("Error", result.Error, true))
            {
                _logger.LogError("[RequestLoggingPipelineBehavior]:: Completed {RequestName} with error", requestName);
            }
        }

        return result;
    }
}
