using YGZ.Catalog.Domain.Core.Errors;

namespace YGZ.Catalog.Domain.Core.Abstractions.Result.Validations;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "Validation error");

    Error[] Errors { get; }
}