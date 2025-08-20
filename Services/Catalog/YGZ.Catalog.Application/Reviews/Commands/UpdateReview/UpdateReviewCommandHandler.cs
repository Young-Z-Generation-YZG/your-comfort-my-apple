
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, bool>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRequestContext _userContext;

    public UpdateReviewCommandHandler(IReviewRepository reviewRepository, IUserRequestContext userContext)
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<Review>.Filter.Eq(x => x.Id, ReviewId.Of(request.ReviewId));
        filter &= Builders<Review>.Filter.Eq(x => x.CustomerId, _userContext.GetUserId());

        var review = await _reviewRepository.GetByFilterAsync(filter, cancellationToken);

        if(review is null)
        {
            return Errors.Review.NotFound;
        }

        var oldReview = Review.Create(content: review.Content,
                                      rating: review.Rating,
                                      productId: review.ProductId,
                                      modelId: review.ModelId,
                                      OrderId: review.OrderId,
                                      orderItemId: review.OrderItemId,
                                      customerId: review.CustomerId,
                                      customerUserName: review.CustomerUserName);

        review.Update(request.Content, request.Rating, oldReview);

        var result = await _reviewRepository.UpdateAsync(request.ReviewId, review, cancellationToken);

        return result;
    }
}
