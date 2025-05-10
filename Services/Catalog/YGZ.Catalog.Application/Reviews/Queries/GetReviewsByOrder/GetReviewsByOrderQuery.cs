


using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Reviews;

namespace YGZ.Catalog.Application.Reviews.Queries.GetReviewsByOrder;

public sealed record GetReviewsByOrderQuery(string OrderId) : IQuery<List<ReviewInOrderResponse>> { }