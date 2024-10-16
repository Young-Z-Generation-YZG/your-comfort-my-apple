
using YGZ.Identity.Domain.Common.Errors;

namespace YGZ.Identity.Domain.Common.Abstractions.Validations;

public class ValidationResult<TResponse> : Result<TResponse>, IValidationResult
{
    public Error[] Errors { get; }

    public ValidationResult(Error[] errors) : base(false, default!, Error.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult<TResponse> WithErrors(Error[] errors) => new(errors);
}
    