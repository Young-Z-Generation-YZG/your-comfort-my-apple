

using MediatR;
using YGZ.Catalog.Domain.Products.Iphone.Events;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewCreatedDomainEventHandler : INotificationHandler<ReviewCreatedDomainEvent>
{
    //private readonly IIPhone16ModelRepository _modelRepository;
    //private readonly IMongoRepository<IPhone16Detail, IphoneId> _iPhoneRepository;

    //public ReviewCreatedDomainEventHandler(IMongoRepository<IPhone16Detail, IPhone16Id> iPhoneRepository, IIPhone16ModelRepository modelRepository)
    //{
    //    _iPhoneRepository = iPhoneRepository;
    //    _modelRepository = modelRepository;
    //}

    public async Task Handle(ReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        //var iphone = await _iPhoneRepository.GetByIdAsync(notification.Review.ProductId.Value!, cancellationToken);

        //var model = await _modelRepository.GetByIdAsync(iphone.IPhoneModelId.Value!, cancellationToken);

        //model.AddNewRating(notification.Review);

        //await _modelRepository.UpdateAsync(model.Id.Value!, model, cancellationToken);
    }
}
