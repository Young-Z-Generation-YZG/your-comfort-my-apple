
namespace YGZ.Identity.Contracts.Identity.Login;

public sealed record LoginResponse(string Email, string Fullname, string AccessToken, string RefreshToken) { }