

using MediatR;
using YGZ.Ordering.Domain.Core.Abstractions.Result;

namespace YGZ.Ordering.Application.Core.Abstractions.Messaging;

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse> { }
