
using MediatR;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Core.Abstractions.Messaging;
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

