

using YGZ.Ordering.Domain.Core.Errors;

namespace YGZ.Ordering.Domain.Core.Abstractions.Result;

public class ValidationResult<TResponse> : Result<TResponse>, IValidationResult
{
    public Error[] Errors { get; }

    public ValidationResult(Error[] errors) : base(false, default!, Error.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult<TResponse> WithErrors(Error[] errors) => new(errors);
}
