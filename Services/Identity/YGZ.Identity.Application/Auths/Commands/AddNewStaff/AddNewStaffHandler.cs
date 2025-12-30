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
    private readonly IUserRegistrationCompensator _compensator;

    public AddNewStaffHandler(ILogger<AddNewStaffHandler> logger,
                              IIdentityService identityService,
                              IKeycloakService keycloakService,
                              UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IPasswordHasher<User> passwordHasher,
                              IUserRegistrationCompensator compensator)
    {
        _logger = logger;
        _identityService = identityService;
        _keycloakService = keycloakService;
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordHasher = passwordHasher;
        _compensator = compensator;
    }

    public async Task<Result<bool>> Handle(AddNewStaffCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingUserResult = await _identityService.FindUserAsync(request.Email, ignoreBaseFilter: true);
            if (existingUserResult.IsSuccess && existingUserResult.Response is not null)
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(_identityService.FindUserAsync), "User already exists", new { request.Email });
                
                return Errors.User.AlreadyExists;
            }

            // Validate role exists in both systems
            var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(_roleManager.RoleExistsAsync), "Role does not exist", new { request.RoleName });
                
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

            // Step 1: Create user in Keycloak
            string keycloakUserId;
            try
            {
                keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(userRepresentation);
                _compensator.TrackStep(RegistrationStep.KeycloakUserCreated, keycloakUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.CreateKeycloakUserAsync), ex.Message, new { request.Email });
                
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

            // Step 2: Create user in database
            try
            {
                var createResult = await _userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_userManager.CreateAsync), "Failed to create user in database", new { request.Email, Errors = createResult.Errors });

                    await _compensator.CompensateAsync(keycloakUserId, null, cancellationToken);
                    return Errors.User.CannotBeCreated;
                }
                
                _compensator.TrackStep(RegistrationStep.DatabaseUserCreated, newUser.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.CreateAsync), ex.Message, new { request.Email });
                
                await _compensator.CompensateAsync(keycloakUserId, null, cancellationToken);
                return Errors.User.CannotBeCreated;
            }

            // Step 3: Assign the requested role to user in database
            var roleResult = await _userManager.AddToRoleAsync(newUser, request.RoleName);
            if (!roleResult.Succeeded)
            {
                _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.AddToRoleAsync), "Failed to assign role to user", new { request.RoleName, request.Email, Errors = roleResult.Errors });

                await _compensator.CompensateAsync(keycloakUserId, newUser.Id.ToString(), cancellationToken);
                return Errors.Keycloak.CannotAssignRole;
            }
            
            _compensator.TrackStep(RegistrationStep.RoleAssigned, newUser.Id);

            // Step 4: Assign role in Keycloak (after database assignment succeeds)
            var keycloakRoleResult = await _keycloakService.AssignRolesToUserAsync(keycloakUserId, new List<string> { request.RoleName });
            if (keycloakRoleResult.IsFailure)
            {
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.AssignRolesToUserAsync), "Failed to assign role in Keycloak, but user was created in database. Manual intervention may be required",
                    new { keycloakUserId, request.RoleName });
                // User exists in database with role, but Keycloak assignment failed
                // This is acceptable as the user can still function, but Keycloak sync should be monitored
                // Note: We don't rollback here as the user is functional without Keycloak role assignment
            }

            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully created new staff user", new { request.Email, request.RoleName });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { email = request.Email };
            _logger.LogError(ex, ":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            
            // Attempt compensation based on current state
            var state = _compensator.GetState();
            await _compensator.CompensateAsync(state.KeycloakUserId, state.DatabaseUserId, cancellationToken);
            
            throw;
        }
    }
}
