

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Identity.Application.Users.Commands.UpdateAddress;

public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand, bool>
{
    public async Task<Result<bool>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
