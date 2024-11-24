
using MediatR;
using YGZ.Catalog.Domain.Core.Abstractions.Result;

namespace YGZ.Catalog.Application.Core.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }