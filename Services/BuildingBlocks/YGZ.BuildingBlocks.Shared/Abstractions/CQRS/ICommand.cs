

using MediatR;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }