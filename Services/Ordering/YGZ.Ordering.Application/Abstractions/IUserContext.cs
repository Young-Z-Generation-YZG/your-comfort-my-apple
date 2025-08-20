

namespace YGZ.Ordering.Application.Abstractions;

public interface IUserRequestContext
{
    string GetUserEmail();
    string GetUserId();
    List<string> GetUserRoles();
}