using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetProfile;

public sealed record GetMeQuery() : IQuery<GetAccountResponse> { }