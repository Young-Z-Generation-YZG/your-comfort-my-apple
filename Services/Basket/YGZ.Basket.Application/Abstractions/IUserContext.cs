

namespace YGZ.Basket.Application.Abstractions;

public interface IUserContext
{
    string GetUserEmail();
    string GetUserId(); // Optional: for "sub" claim
}