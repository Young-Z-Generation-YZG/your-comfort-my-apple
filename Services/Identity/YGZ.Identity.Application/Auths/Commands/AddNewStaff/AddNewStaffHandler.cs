using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.BuilderClasses;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Application.Auths.Commands.AddNewStaff;

public class AddNewStaffHandler : ICommandHandler<AddNewStaffCommand, bool>
{
    private readonly ILogger<AddNewStaffHandler> _logger;
    private readonly IIdentityService _identityService;
    private readonly IKeycloakService _keycloakService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AddNewStaffHandler(
        ILogger<AddNewStaffHandler> logger,
        IIdentityService identityService,
        IKeycloakService keycloakService,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IPasswordHasher<User> passwordHasher)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<bool>> Handle(AddNewStaffCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists
            var existingUserResult = await _identityService.FindUserByEmailAsync(request.Email);
            if (existingUserResult.Response is not null)
            {
                _logger.LogWarning("User already exists with email: {Email}", request.Email);
                return Errors.User.AlreadyExists;
            }

            // Validate role exists
            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
            {
                _logger.LogWarning("Role does not exist: {RoleName}", request.RoleName);
                return Errors.Keycloak.RoleNotFound;
            }

            // Create user representation for Keycloak
            var userRepresentation = UserRepresentation.CreateBuilder()
                .WithUsername(request.Email)
                .WithEmail(request.Email)
                .WithEnabled(true)
                .WithFirstName(request.FirstName)
                .WithLastName(request.LastName)
                .WithEmailVerified(true) // Staff accounts are pre-verified
                .WithPassword(request.Password)
                .WithTenantAttributes(
                    tenantId: request.TenantId,
                    subDomain: null,
                    tenantType: null,
                    branchId: request.BranchId,
                    fullName: $"{request.FirstName} {request.LastName}"
                )
                .Build();

            // Create user in Keycloak
            var keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(userRepresentation);

            // Create user in database with default values for staff
            var newUser = User.Create(
                guid: new Guid(keycloakUserId),
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                passwordHash: "", // Will be set below
                firstName: request.FirstName,
                lastName: request.LastName,
                birthDay: DateTime.UtcNow.AddYears(-25), // Default birth date (25 years ago)
                image: null,
                country: "VN", // Default country
                emailConfirmed: true, // Staff accounts are pre-verified
                tenantId: request.TenantId,
                branchId: request.BranchId
            );

            // Hash password
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            // Create user in database
            var createResult = await _userManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                _logger.LogError("Failed to create user in database: {Email}, Errors: {Errors}", 
                    request.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                
                // Rollback Keycloak user
                await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
                return Errors.User.CannotBeCreated;
            }

            // Remove USER role if it was assigned (CreateKeycloakUserAsync assigns USER role)
            // and assign the requested role instead
            var currentRoles = await _userManager.GetRolesAsync(newUser);
            if (currentRoles.Contains(AuthorizationConstants.Roles.USER) && request.RoleName != AuthorizationConstants.Roles.USER)
            {
                var removeUserRoleResult = await _userManager.RemoveFromRoleAsync(newUser, AuthorizationConstants.Roles.USER);
                if (!removeUserRoleResult.Succeeded)
                {
                    _logger.LogWarning("Failed to remove USER role from new staff user: {Email}", request.Email);
                }
            }

            // Assign the requested role to user
            var roleResult = await _userManager.AddToRoleAsync(newUser, request.RoleName);
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role {RoleName} to user: {Email}, Errors: {Errors}", 
                    request.RoleName, request.Email, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                
                // Rollback database user
                await _userManager.DeleteAsync(newUser);
                // Rollback Keycloak user
                await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
                return Errors.Keycloak.CannotAssignRole;
            }

            // Assign role in Keycloak
            var keycloakRoleResult = await _keycloakService.AssignRolesToUserAsync(keycloakUserId, new List<string> { request.RoleName });
            if (keycloakRoleResult.IsFailure)
            {
                _logger.LogWarning("Failed to assign role in Keycloak, but user was created. UserId: {UserId}, Role: {RoleName}", 
                    keycloakUserId, request.RoleName);
                // Don't rollback here as the user is already created - role can be assigned later
            }

            _logger.LogInformation("Successfully created new staff user: {Email} with role: {RoleName}", 
                request.Email, request.RoleName);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating new staff user: {Email}", request.Email);
            throw;
        }
    }
}
