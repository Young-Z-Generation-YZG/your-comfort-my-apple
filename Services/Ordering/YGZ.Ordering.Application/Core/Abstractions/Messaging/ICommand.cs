

using MediatR;
using YGZ.Ordering.Domain.Core.Abstractions.Result;

namespace YGZ.Ordering.Application.Core.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }