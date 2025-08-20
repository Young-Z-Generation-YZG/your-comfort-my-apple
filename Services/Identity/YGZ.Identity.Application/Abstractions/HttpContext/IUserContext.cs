
namespace YGZ.Identity.Application.Abstractions.HttpContext;

public interface IUserRequestContext
{
    string GetUserEmail();
    string GetUserId(); // Optional: for "sub" claim
}