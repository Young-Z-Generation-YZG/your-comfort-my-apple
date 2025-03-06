

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Keycloak.Application.Users.Commands;

public class UpdateProfileCommandHandler : ICommandHandler<UpdateProfileCommand, bool>
{
    public Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
