
using YGZ.Identity.Domain.Common.Errors;

namespace YGZ.Identity.Domain.Common.Abstractions.Validations;
public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "Validation error");

    Error[] Errors { get; }
}
