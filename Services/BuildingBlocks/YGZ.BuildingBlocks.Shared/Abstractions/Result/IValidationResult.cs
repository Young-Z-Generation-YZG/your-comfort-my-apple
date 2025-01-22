

using System.Reflection;
using YGZ.BuildingBlocks.Shared.Errors;

namespace YGZ.BuildingBlocks.Shared.Abstractions.Result;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "Validation error", Assembly.GetExecutingAssembly().GetName().Name!);
    Error[] Errors { get; }
}