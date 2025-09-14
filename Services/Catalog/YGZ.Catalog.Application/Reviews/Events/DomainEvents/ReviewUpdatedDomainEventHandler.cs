

//using MediatR;
//using YGZ.Catalog.Application.Abstractions.Data;
//using YGZ.Catalog.Domain.Products.Iphone.Events;
//using YGZ.Catalog.Domain.Products.Iphone16.Entities;
//using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

//namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

//public class ReviewUpdatedDomainEventHandler : INotificationHandler<ReviewUpdatedDomainEvent>
//{
//    private readonly IIPhone16ModelRepository _modelRepository;
//    private readonly IMongoRepository<IPhone16Detail, IPhone16Id> _iPhoneRepository;

//    public ReviewUpdatedDomainEventHandler(IMongoRepository<IPhone16Detail, IPhone16Id> iPhoneRepository, IIPhone16ModelRepository modelRepository)
//    {
//        _iPhoneRepository = iPhoneRepository;
//        _modelRepository = modelRepository;
//    }

//    public async Task Handle(ReviewUpdatedDomainEvent notification, CancellationToken cancellationToken)
//    {
//        //var model = await _modelRepository.GetByIdAsync(notification.oldReview.ModelId.Value!, cancellationToken);

//        //model.UpdateRating(notification.oldReview, notification.newReview);

//        //await _modelRepository.UpdateAsync(model.Id.Value!, model, cancellationToken);
//    }
//}
