using System.Diagnostics.CodeAnalysis;

namespace YGZ.Identity.Api.Contracts.Users;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Underscore prefix is used for query parameters matching API conventions")]
public sealed record GetUsersPaginationRequest()
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public string? _email { get; init; }
    public string? _firstName { get; init; }
    public string? _lastName { get; init; }
    public string? _phoneNumber { get; init; }
};
