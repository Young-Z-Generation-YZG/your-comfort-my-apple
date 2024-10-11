
using MediatR;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Common.Abstractions.Messaging;
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
