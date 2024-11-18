

using MediatR;
using YGZ.Basket.Domain.Core.Abstractions.Result;

namespace YGZ.Basket.Application.Core.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse> { }
