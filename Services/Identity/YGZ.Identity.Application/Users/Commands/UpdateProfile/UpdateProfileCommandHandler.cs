using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfile;


public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
