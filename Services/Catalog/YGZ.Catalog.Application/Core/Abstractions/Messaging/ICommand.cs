
using MediatR;

namespace YGZ.Catalog.Application.Core.Abstractions.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }