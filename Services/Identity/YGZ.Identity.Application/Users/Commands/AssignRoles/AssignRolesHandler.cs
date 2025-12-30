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
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "No roles provided for user", new { request.UserId });
                
                return Errors.Keycloak.RoleNotFound;
            }

            if (!Guid.TryParse(request.UserId, out _))
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(Guid.TryParse), "Invalid user ID format", new { request.UserId });
                
                return Errors.User.DoesNotExist;
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.FindByIdAsync), "User not found", new { request.UserId });
                
                return Errors.User.DoesNotExist;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_userManager.RemoveFromRolesAsync), "Failed to remove existing roles from user", new { request.UserId, Errors = removeResult.Errors });
                    
                    return Errors.Keycloak.CannotAssignRole;
                }
            }

            var addRolesResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!addRolesResult.Succeeded)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.AddToRolesAsync), "Failed to assign roles via UserManager", new { request.UserId, Errors = addRolesResult.Errors });

                if (currentRoles.Any())
                {
                    var restoreResult = await _userManager.AddToRolesAsync(user, currentRoles);
                    if (!restoreResult.Succeeded)
                    {
                        _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                            nameof(_userManager.AddToRolesAsync), "Failed to restore previous roles after failed assignment", new { request.UserId });
                    }
                }

                return Errors.Keycloak.CannotAssignRole;
            }

            var keycloakResult = await _keycloakService.AssignRolesToUserAsync(request.UserId, request.Roles);
            if (keycloakResult.IsFailure)
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.AssignRolesToUserAsync), "Failed to assign roles via Keycloak. User has roles in database but not in Keycloak. Manual intervention may be required",
                    new { request.UserId, Error = keycloakResult.Error });
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully assigned roles to user", new { request.UserId, request.Roles });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { userId = request.UserId };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            throw;
        }
    }
}

