
namespace YGZ.Identity.Application.Abstractions.HttpContext;

public interface IUserContext
{
    string GetUserEmail();
    string GetUserId(); // Optional: for "sub" claim
}