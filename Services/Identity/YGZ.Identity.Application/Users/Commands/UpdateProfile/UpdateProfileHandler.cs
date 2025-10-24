using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfile;


public class UpdateProfileHandler : ICommandHandler<UpdateProfileCommand, bool>
{
    private readonly IUserHttpContext _userHttpContext;
    private readonly ILogger<UpdateProfileHandler> _logger;
    private readonly IIdentityDbContext _dbContext;
    private readonly DbSet<User> _userDbSet;
    private readonly DbSet<Profile> _profileDbSet;

    public UpdateProfileHandler(IUserHttpContext userHttpContext,
                                ILogger<UpdateProfileHandler> logger,
                                IIdentityDbContext dbContext)
    {
        _userHttpContext = userHttpContext;
        _logger = logger;
        _dbContext = dbContext;
        _userDbSet = dbContext.Users;
        _profileDbSet = dbContext.Profiles;
    }

    public async Task<Result<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();

        var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = await _userDbSet.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken) ?? throw new Exception("User not found");

            user.PhoneNumber = request.PhoneNumber;

            _userDbSet.Update(user);

            var profile = await _profileDbSet.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken) ?? throw new Exception("Profile not found");

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.BirthDay = DateTime.Parse(request.BirthDay).ToUniversalTime();
            profile.SetGender(request.Gender);

            _profileDbSet.Update(profile);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await transaction.CommitAsync(cancellationToken);
                return true;
            }

            await transaction.RollbackAsync(cancellationToken);
            return false;

        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }
}
