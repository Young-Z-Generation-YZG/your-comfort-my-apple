

using YGZ.Basket.Domain.Errors;

namespace YGZ.Basket.Domain.Core.Abstractions.Result;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "Validation error");

    Error[] Errors { get; }
}
