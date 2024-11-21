

using MediatR;
using YGZ.Ordering.Domain.Core.Abstractions.Result;

namespace YGZ.Ordering.Application.Core.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse> { }