

namespace YGZ.Catalog.Application.Abstractions;

public interface IUserRequestContext
{
    string GetUserEmail();
    string GetUserId(); // Optional: for "sub" claim
}