
using MediatR;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse> { }