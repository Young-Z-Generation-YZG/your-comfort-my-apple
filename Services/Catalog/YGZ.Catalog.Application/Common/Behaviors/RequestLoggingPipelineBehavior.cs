
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Common.Behaviors;

public class RequestLoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
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

        _logger.LogInformation(
            "Handling {RequestName} with request data: {@Request}", typeof(TRequest).Name, request);

        TResponse result = await next();

        if (result is Result<object> resultResponse)
        {
            if (resultResponse.IsFailure)
            {
                //using (LogContext.PushProperty("Error", result.Errors, true))
                //{
                //}
                _logger.LogError(
                    "Handled {RequestName} with errors: {@Errors}", typeof(TRequest).Name, resultResponse.Error);

            }
            else
            {
                _logger.LogInformation("Handled {RequestName} successfully with response data: {@Response}", typeof(TRequest).Name, resultResponse.Response);
            }
        }
        else
        {
            _logger.LogInformation("Handled {RequestName} with response data: {@Response}", typeof(TRequest).Name, result);
        }

        return result;
    }
}
