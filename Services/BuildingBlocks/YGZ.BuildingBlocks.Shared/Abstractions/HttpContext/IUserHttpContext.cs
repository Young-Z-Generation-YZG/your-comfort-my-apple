namespace YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;

public interface IUserHttpContext
{
    string GetUserId();
    string GetUserEmail();
    List<string> GetUserRoles();
}
