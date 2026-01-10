
using System.Reflection;

namespace YGZ.BuildingBlocks.Shared.Errors;

public class Error
{
    public string Code { get; set; }    
    public string Message { get; set; }
    private string ServiceName { get; set; }

    public Error(string code, string message, string serviceName)
    {
        Code = code;
        Message = message;
        ServiceName = serviceName;
    }

    public static bool operator ==(Error left, Error right)
    {
        return left.Code == right.Code;
    }

    public static bool operator !=(Error left, Error right)
    {
        return left.Code != right.Code;
    }

    public static Error UnknownError => new(GetAssemblyName() + "Test", "Unknown error", GetAssemblyName());
    public static Error NoneError => new(GetAssemblyName() + "Test", "None error", GetAssemblyName());
    public static Error ValidationError => new(GetAssemblyName() + "Test", "Validation error", GetAssemblyName());
    public static Error GrpcError(string code, string message) => new("Test", message, GetAssemblyName());
    public static Error BadRequest(string code, string message, string serviceName) => new(code, message, serviceName);
    public static Error NotFound(string code, string message, string serviceName) => new(code, message, serviceName);
    public static Error Validation(string code, string message, string serviceName) => new(code, message, serviceName);
    public static Error OperationFailed(string code, string message, string serviceName) => new(code, message, serviceName);

    private static string GetAssemblyName()
    {
        return Assembly.GetExecutingAssembly().GetName().Name!;
    }


}
