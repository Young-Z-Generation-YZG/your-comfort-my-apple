

using MediatR;
using YGZ.Ordering.Domain.Core.Abstractions.Result;

namespace YGZ.Ordering.Application.Core.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
