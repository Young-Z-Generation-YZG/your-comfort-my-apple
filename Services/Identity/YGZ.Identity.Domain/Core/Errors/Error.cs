namespace YGZ.Identity.Domain.Common.Errors;
public class Error
{
    public string Code { get; set; }
    public string Message { get; set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static Error NoneError => new("Identity.NoneError", "None error");
    public static Error ValidationError => new("Identity.ValidationError", "Validation error");
    public static Error BadRequest(string code, string message) => new(code, message);
}
