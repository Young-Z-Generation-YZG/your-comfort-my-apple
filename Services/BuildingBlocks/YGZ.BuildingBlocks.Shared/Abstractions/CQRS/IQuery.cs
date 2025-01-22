

using MediatR;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }