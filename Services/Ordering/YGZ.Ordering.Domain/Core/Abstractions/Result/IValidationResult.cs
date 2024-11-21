

using YGZ.Ordering.Domain.Core.Errors;

namespace YGZ.Ordering.Domain.Core.Abstractions.Result;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "Validation error");

    Error[] Errors { get; }
}