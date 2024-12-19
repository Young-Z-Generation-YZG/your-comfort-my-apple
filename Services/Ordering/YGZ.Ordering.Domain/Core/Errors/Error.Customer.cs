

namespace YGZ.Ordering.Domain.Core.Errors;

public static partial class Errors
{
    public static class Customer
    {
        public static Error IdInvalid = Error.BadRequest(code: "Customer.IdInvalid", message: "Customer Id is invalid format Guid");
        public static Error DoesNotExist = Error.BadRequest(code: "Customer.DoesNotExist", message: "Customer does not Exists");
    }
}
