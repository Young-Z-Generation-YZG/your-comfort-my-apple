

using MediatR;
using YGZ.Basket.Domain.Core.Abstractions.Result;

namespace YGZ.Basket.Application.Core.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse> { }