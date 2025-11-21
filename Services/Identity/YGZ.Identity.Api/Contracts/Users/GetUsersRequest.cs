namespace YGZ.Identity.Api.Contracts.Users;

public sealed record GetUsersRequest
{
    public int? _page { get; init; }
    public int? _limit { get; init; }
    public string? _email { get; init; }
    public string? _firstName { get; init; }
    public string? _lastName { get; init; }
    public string? _phoneNumber { get; init; }
}