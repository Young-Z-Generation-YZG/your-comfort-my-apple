﻿using Microsoft.AspNetCore.Mvc;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Api.Common.Extensions;

public static class ResultExtension
{
    public static IActionResult Match<TResponse>(
            this Result<TResponse> result,
                Func<TResponse, IActionResult> onSuccess,
                Func<Result<TResponse>, IActionResult> onFailure)
    {
        if (result is null)
            throw new NullReferenceException("Object Result is null");

        return result.IsSuccess ? onSuccess(result.Response!) : onFailure(result);
    }
}