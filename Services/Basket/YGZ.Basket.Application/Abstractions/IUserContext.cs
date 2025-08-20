

namespace YGZ.Basket.Application.Abstractions;

public interface IUserRequestContext
{
    string GetUserEmail();
    string GetUserId(); // Optional: for "sub" claim
}