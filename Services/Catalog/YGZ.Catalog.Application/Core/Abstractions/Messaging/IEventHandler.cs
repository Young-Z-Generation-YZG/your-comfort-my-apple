

using MediatR;

namespace YGZ.Catalog.Application.Core.Abstractions.Messaging;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : INotification { }
