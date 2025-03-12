

namespace YGZ.Ordering.Application.Abstractions;

public interface IUserContext
{
    string GetUserEmail();
    string GetUserId(); // Optional: for "sub" claim
}