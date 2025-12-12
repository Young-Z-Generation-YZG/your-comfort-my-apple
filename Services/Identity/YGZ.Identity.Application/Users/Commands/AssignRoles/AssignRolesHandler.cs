using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Users.Commands.AssignRoles;

public class AssignRolesHandler : ICommandHandler<AssignRolesCommand, bool>
{
    private readonly ILogger<AssignRolesHandler> _logger;
    private readonly IKeycloakService _keycloakService;
    private readonly IIdentityService _identityService;
    private readonly UserManager<User> _userManager;

    public AssignRolesHandler(ILogger<AssignRolesHandler> logger,
                              IKeycloakService keycloakService,
                              IIdentityService identityService,
                              UserManager<User> userManager)
    {
        _logger = logger;
        _keycloakService = keycloakService;
        _identityService = identityService;
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(AssignRolesCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Roles == null || !request.Roles.Any())
            {
                _logger.LogWarning("No roles provided for user {UserId}", request.UserId);
                return Errors.Keycloak.RoleNotFound;
            }

            if (!Guid.TryParse(request.UserId, out _))
            {
                _logger.LogWarning("Invalid user ID format: {UserId}", request.UserId);
                return Errors.User.DoesNotExist;
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", request.UserId);
                return Errors.User.DoesNotExist;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    _logger.LogError("Failed to remove existing roles from user {UserId}. Errors: {Errors}",
                        request.UserId, string.Join(", ", removeResult.Errors.Select(e => e.Description)));
                    return Errors.Keycloak.CannotAssignRole;
                }
            }

            var addRolesResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!addRolesResult.Succeeded)
            {
                _logger.LogError("Failed to assign roles via UserManager for user {UserId}. Errors: {Errors}",
                    request.UserId, string.Join(", ", addRolesResult.Errors.Select(e => e.Description)));

                if (currentRoles.Any())
                {
                    var restoreResult = await _userManager.AddToRolesAsync(user, currentRoles);
                    if (!restoreResult.Succeeded)
                    {
                        _logger.LogError("Failed to restore previous roles for user {UserId} after failed assignment", request.UserId);
                    }
                }

                return Errors.Keycloak.CannotAssignRole;
            }

            var keycloakResult = await _keycloakService.AssignRolesToUserAsync(request.UserId, request.Roles);
            if (keycloakResult.IsFailure)
            {
                _logger.LogWarning("Failed to assign roles via Keycloak for user {UserId}: {Error}. User has roles in database but not in Keycloak. Manual intervention may be required.",
                    request.UserId, keycloakResult.Error);
            }

            _logger.LogInformation("Successfully assigned roles {Roles} to user {UserId}",
                string.Join(", ", request.Roles), request.UserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while assigning roles to user {UserId}", request.UserId);
            throw;
        }
    }
}

