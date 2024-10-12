
using MediatR;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Common.Abstractions.Messaging;
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse> { }
