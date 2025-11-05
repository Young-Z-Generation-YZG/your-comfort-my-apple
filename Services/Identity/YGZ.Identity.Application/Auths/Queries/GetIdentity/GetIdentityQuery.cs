using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.Identity.Application.Auths.Queries.GetIdentity;

public sealed record GetIdentityQuery() : IQuery<GetIdentityResponse>;