using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class UpdateReviewHandler : ICommandHandler<UpdateReviewCommand, bool>
{
    private readonly IReviewRepository _reviewRepository;

    public UpdateReviewHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<bool>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {

        var filter = Builders<Review>.Filter.Eq(x => x.Id, ReviewId.Of(request.ReviewId));

        var review = await _reviewRepository.GetByFilterAsync(filter, cancellationToken);

        if (review is null)
        {
            return Errors.Review.NotFound;
        }

        var oldReview = await _reviewRepository.GetByFilterAsync(filter, cancellationToken);

        if (oldReview is null)
        {
            return Errors.Review.NotFound;
        }

        review.Update(request.Content, request.Rating, oldReview);

        var result = await _reviewRepository.UpdateAsync(request.ReviewId, review);

        return result;
    }
}
