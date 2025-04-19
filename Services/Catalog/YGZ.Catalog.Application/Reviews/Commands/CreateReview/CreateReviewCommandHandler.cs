

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Catalog.Application.Abstractions;
using YGZ.Catalog.Application.Reviews.Extensions;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Application.Reviews.Commands;

public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, bool>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserContext _userContext;

    public CreateReviewCommandHandler(IReviewRepository reviewRepository, IUserContext userContext)
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
    }

    public async Task<Result<bool>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        //var userId = _userContext.GetUserId();
        var customerId = "ed04b044-86de-475f-9122-d9807897f969";

        var review = request.ToEntity(customerId);

        var result = await _reviewRepository.InsertOneAsync(review);

        if(result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
