

using MediatR;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

public interface IQueryHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : IQuery<TResponse> { }