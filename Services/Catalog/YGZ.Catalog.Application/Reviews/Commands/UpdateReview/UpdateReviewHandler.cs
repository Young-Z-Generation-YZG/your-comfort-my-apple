using Microsoft.Extensions.Logging;
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
    private readonly ILogger<UpdateReviewHandler> _logger;
    private readonly IMongoRepository<Review, ReviewId> _reviewRepository;

    public UpdateReviewHandler(IMongoRepository<Review, ReviewId> reviewRepository, ILogger<UpdateReviewHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken, ignoreBaseFilter: true);

        if (review is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_reviewRepository.GetByIdAsync), "Review not found", new { reviewId = request.ReviewId });

            return Errors.Review.NotFound;
        }

        var oldReview = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken, ignoreBaseFilter: true);

        if (oldReview is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_reviewRepository.GetByIdAsync), "Old review not found", new { reviewId = request.ReviewId });

            return Errors.Review.NotFound;
        }

        review.Update(request.Content, request.Rating, oldReview);

        var result = await _reviewRepository.UpdateAsync(request.ReviewId, review, ignoreBaseFilter: true);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_reviewRepository.UpdateAsync), "Failed to update review", new { reviewId = request.ReviewId, rating = request.Rating, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully updated review", new { reviewId = request.ReviewId, rating = request.Rating });

        return result;
    }
}
