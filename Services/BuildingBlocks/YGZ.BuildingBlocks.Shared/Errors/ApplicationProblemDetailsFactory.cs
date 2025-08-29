

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using FluentValidation;

namespace YGZ.BuildingBlocks.Shared.Errors;

public class ApplicationProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _apiBehaviorOptions;
    private readonly ILogger<ApplicationProblemDetailsFactory> _logger;

    public ApplicationProblemDetailsFactory(IOptions<ApiBehaviorOptions>
        options, ILogger<ApplicationProblemDetailsFactory> logger)
    {
        _apiBehaviorOptions = options.Value;
        _logger = logger;
    }

    public override ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Type = type,
            Detail = detail,
            Instance = instance
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ModelStateDictionary modelStateDictionary,
        int? statusCode = null,
        string? title = null,
        string? type = null,
        string? detail = null,
        string? instance = null)
    {
        if (modelStateDictionary == null)
        {
            throw new ArgumentNullException(nameof(modelStateDictionary));
        }
        statusCode ??= 400;
        var problemDetails = new ValidationProblemDetails(modelStateDictionary)
        {
            Status = statusCode,
            Type = type,
            Detail = detail,
            Instance = instance,
        };
        if (title != null)
        {
            problemDetails.Title = title;
        }
        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);
        return problemDetails;
    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_apiBehaviorOptions.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        if (traceId != null)
        {
            problemDetails.Extensions["trace_id"] = traceId;
        }

        var path = httpContext?.Request?.Path.Value;

        if (!string.IsNullOrEmpty(path))
        {
            problemDetails.Extensions["path"] = path;
        }

        if (httpContext?.Items["errors"] is Error[] errors)
        {
            problemDetails.Extensions.Add("errors", errors);

            var ex = new ValidationException("Validation failed");

            _logger.LogError(ex, "Exception occurred: {@Title} {@Errors} {@Exception} {@TraceId} {@DateTimeUtc}", ex.Message, errors, ex, traceId, DateTime.UtcNow);
        }
        if (httpContext?.Items["error"] is Error error)
        {
            problemDetails.Extensions.Add("error", error);

            _logger.LogError("Exception occurred: {@Title} {@Error} {@ProblemDetails} {@TraceId} {@DateTimeUtc}", error.Message, error, problemDetails, traceId, DateTime.UtcNow);
        }
    }
}
