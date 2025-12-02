using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Auths.Commands.AssignRoles;

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
            // Validate user ID format
            if (!Guid.TryParse(request.UserId, out var userId))
            {
                _logger.LogWarning("Invalid user ID format: {UserId}", request.UserId);
                return Errors.User.DoesNotExist;
            }

            // Find user in database
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", request.UserId);
                return Errors.User.DoesNotExist;
            }

            // Assign roles via Keycloak
            var keycloakResult = await _keycloakService.AssignRolesToUserAsync(request.UserId, request.Roles);
            if (keycloakResult.IsFailure)
            {
                _logger.LogError("Failed to assign roles via Keycloak for user {UserId}: {Error}",
                    request.UserId, keycloakResult.Error);
                return keycloakResult.Error;
            }

            // Get current user roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all existing roles
            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    _logger.LogWarning("Failed to remove existing roles from user {UserId}", request.UserId);
                }
            }

            // Add new roles via ASP.NET Identity UserManager
            var addRolesResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!addRolesResult.Succeeded)
            {
                _logger.LogError("Failed to assign roles via UserManager for user {UserId}. Errors: {Errors}",
                    request.UserId, string.Join(", ", addRolesResult.Errors.Select(e => e.Description)));
                return Errors.Keycloak.CannotAssignRole;
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
