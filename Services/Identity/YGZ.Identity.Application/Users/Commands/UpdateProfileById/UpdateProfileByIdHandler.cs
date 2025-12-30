using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Application.Users.Commands.UpdateProfileById;

public class UpdateProfileByIdHandler : ICommandHandler<UpdateProfileByIdCommand, bool>
{
    private readonly ILogger<UpdateProfileByIdHandler> _logger;
    private readonly IIdentityDbContext _dbContext;
    private readonly DbSet<User> _userDbSet;
    private readonly DbSet<Profile> _profileDbSet;

    public UpdateProfileByIdHandler(
        ILogger<UpdateProfileByIdHandler> logger,
        IIdentityDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userDbSet = dbContext.Users;
        _profileDbSet = dbContext.Profiles;
    }

    public async Task<Result<bool>> Handle(UpdateProfileByIdCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get user with profile included
            var user = await _userDbSet
                .IgnoreQueryFilters()
                .Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    "FirstOrDefaultAsync", "User not found", new { request.UserId });

                return Errors.User.DoesNotExist;
            }

            if (user.Profile is null)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Profile not found for user", new { request.UserId });

                return Errors.Profile.DoesNotExist;
            }

            var profile = user.Profile;

            // Update user's phone number
            user.PhoneNumber = request.PhoneNumber;
            _userDbSet.Update(user);

            // Update profile using domain method
            profile.Update(request.FirstName, request.LastName, request.PhoneNumber, request.BirthDay, request.Gender);

            _profileDbSet.Update(profile);

            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "Successfully updated profile by user ID", new { request.UserId });

                return true;
            }

            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_dbContext.SaveChangesAsync), "Failed to save changes", new { request.UserId });

            return false;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            var parameters = new { userId = request.UserId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);

            throw;
        }
    }
}
