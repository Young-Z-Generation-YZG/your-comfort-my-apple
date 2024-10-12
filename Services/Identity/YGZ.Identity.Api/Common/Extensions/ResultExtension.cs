using Microsoft.AspNetCore.Mvc;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Api.Extensions.NewFolder
{
    public static class ResultExtension
    {
        public static IActionResult Match<TResponse>(this Result<TResponse> result, Func<TResponse, IActionResult> onSuccess, Func<Result<TResponse>, IActionResult> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Response!) : onFailure(result);
        }
    }
}
