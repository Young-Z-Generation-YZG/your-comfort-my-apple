
using MediatR;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Core.Abstractions.Messaging;
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
