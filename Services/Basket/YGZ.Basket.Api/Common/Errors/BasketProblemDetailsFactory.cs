using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using YGZ.Basket.Domain.Core.Errors;

namespace YGZ.Basket.Api.Common.Errors;

public class BasketProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _apiBehaviorOptions;

    public BasketProblemDetailsFactory(IOptions<ApiBehaviorOptions>
        options)
    {
        _apiBehaviorOptions = options.Value;
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
            problemDetails.Extensions["traceId"] = traceId;
        }

        var path = httpContext?.Request?.Path.Value;

        if (!string.IsNullOrEmpty(path))
        {
            problemDetails.Extensions["path"] = path;
        }

        if (httpContext?.Items["errors"] is Error[] errors)
        {
            problemDetails.Extensions.Add("errors", errors);
        }
        if (httpContext?.Items["error"] is Error error)
        {
            problemDetails.Extensions.Add("error", error);
        }

        //var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;
        //if (errors is not null)
        //{
        //    problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
        //}
    }
}
