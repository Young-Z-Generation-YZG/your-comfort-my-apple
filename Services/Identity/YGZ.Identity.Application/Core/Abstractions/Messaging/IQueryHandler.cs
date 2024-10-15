
using MediatR;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Core.Abstractions.Messaging;
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse> { }
