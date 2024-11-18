
using MediatR;
using YGZ.Basket.Domain.Core.Abstractions.Result;

namespace YGZ.Basket.Application.Core.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }