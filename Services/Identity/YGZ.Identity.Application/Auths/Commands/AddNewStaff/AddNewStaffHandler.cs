using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
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

    public AddNewStaffHandler(ILogger<AddNewStaffHandler> logger,
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
            // Check if user already exists (ignore tenant filters to honor global unique email)
            var existingUserResult = await _identityService.FindUserAsyncIgnoreFilters(request.Email);
            if (existingUserResult.IsSuccess && existingUserResult.Response is not null)
            {
                _logger.LogWarning("User already exists with email: {Email}", request.Email);
                return Errors.User.AlreadyExists;
            }

            // Validate role exists in both systems
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
            string keycloakUserId = string.Empty;
            try
            {
                keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(userRepresentation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user in Keycloak: {Email}", request.Email);
                return Errors.User.CannotBeCreated;
            }

            // Create user in database with default values for staff
            // BirthDay now comes as string; parse and normalize to UTC
            if (!DateTime.TryParse(
                    request.BirthDay,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out var birthDayUtc))
            {
                birthDayUtc = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            }

            var newUser = User.Create(
                guid: new Guid(keycloakUserId),
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                passwordHash: "", // Will be set below
                firstName: request.FirstName,
                lastName: request.LastName,
                birthDay: birthDayUtc,
                image: null,
                country: "VN", // Default country
                emailConfirmed: true, // Staff accounts are pre-verified
                tenantId: request.TenantId,
                branchId: request.BranchId
            );

            // Hash password
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            // Create user in database
            try
            {
                var createResult = await _userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    _logger.LogError("Failed to create user in database: {Email}, Errors: {Errors}",
                        request.Email, string.Join(", ", createResult.Errors.Select(e => e.Description)));

                    // Rollback Keycloak user
                    try
                    {
                        await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to rollback Keycloak user during database creation failure: {UserId}", keycloakUserId);
                    }
                    return Errors.User.CannotBeCreated;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user in database: {Email}", request.Email);
                return Errors.User.CannotBeCreated;
            }

            // Assign the requested role to user in database first
            var roleResult = await _userManager.AddToRoleAsync(newUser, request.RoleName);
            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role {RoleName} to user: {Email}, Errors: {Errors}",
                    request.RoleName, request.Email, string.Join(", ", roleResult.Errors.Select(e => e.Description)));

                // Rollback database user
                try
                {
                    await _userManager.DeleteAsync(newUser);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to rollback database user during role assignment failure: {Email}", request.Email);
                }

                // Rollback Keycloak user
                try
                {
                    await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to rollback Keycloak user during role assignment failure: {UserId}", keycloakUserId);
                }
                return Errors.Keycloak.CannotAssignRole;
            }

            // Assign role in Keycloak (after database assignment succeeds)
            var keycloakRoleResult = await _keycloakService.AssignRolesToUserAsync(keycloakUserId, new List<string> { request.RoleName });
            if (keycloakRoleResult.IsFailure)
            {
                _logger.LogWarning("Failed to assign role in Keycloak, but user was created in database. UserId: {UserId}, Role: {RoleName}. Manual intervention may be required.",
                    keycloakUserId, request.RoleName);
                // User exists in database with role, but Keycloak assignment failed
                // This is acceptable as the user can still function, but Keycloak sync should be monitored
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
