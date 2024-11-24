

namespace YGZ.Ordering.Domain.Core.Errors;

public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error NoneError => new("Order.NoneError", "None error");
    public static Error ValidationError => new("Order.ValidationError", "Validation error");
    public static Error BadRequest(string code, string message) => new(code, message);
}
