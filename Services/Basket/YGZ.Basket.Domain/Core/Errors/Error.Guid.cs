
namespace YGZ.Basket.Domain.Core.Errors;

public static partial class Errors
{
    public static class Guid
    {
        public static Error IdInvalid = Error.BadRequest(code: "Guid.IdInvalid", message: "Guid invalid");
    }
}