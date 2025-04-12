

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;

namespace YGZ.Identity.Application.Users.Queries.GetAddresses;

public sealed record GetAddressesQuery() : IQuery<List<AddressResponse>> { }