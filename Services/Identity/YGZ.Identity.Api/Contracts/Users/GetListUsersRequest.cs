namespace YGZ.Identity.Api.Contracts.Users;

public sealed record GetListUsersRequest
{
    public List<string>? _roles { get; init; }
}
