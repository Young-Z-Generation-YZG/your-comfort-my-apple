using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Errors;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.BuilderClasses;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Seeds;

/// <summary>
/// Service for seeding users following the RegisterHandler pattern:
/// 1. Create Keycloak user to get ID
/// 2. Create Identity user in database
/// 3. Update tenant and branch IDs
/// 4. Assign roles
/// </summary>
public class SeedService
{
    private readonly ILogger<SeedService> _logger;
    private readonly IKeycloakService _keycloakService;
    private readonly IIdentityService _identityService;
    private readonly UserManager<User> _userManager;
    private const string DEFAULT_PASSWORD = "password"; // Default password for seed users

    public SeedService(
        ILogger<SeedService> logger,
        IKeycloakService keycloakService,
        IIdentityService identityService,
        UserManager<User> userManager)
    {
        _logger = logger;
        _keycloakService = keycloakService;
        _identityService = identityService;
        _userManager = userManager;
    }

    /// <summary>
    /// Seeds users from SeedUsers.UsersTenant_YBZONE following RegisterHandler pattern
    /// </summary>
    public async Task<Result<bool>> SeedUsersTenantYBZONEAsync(CancellationToken cancellationToken = default)
    {
        return Result<bool>.Success(true);
        // try
        // {
        //     var users = SeedUsers.UsersTenant_YBZONE.ToList();
        //     var userRoles = GetUserRolesForYBZONE();

        //     _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //         nameof(SeedUsersTenantYBZONEAsync), "Starting to seed YBZONE users", new { UserCount = users.Count });

        //     foreach (var user in users)
        //     {
        //         try
        //         {
        //             // Step 1: Check if user already exists
        //             var existingUserResult = await _identityService.FindUserByEmailAsync(user.Email!);
        //             if (existingUserResult.IsSuccess && existingUserResult.Response is not null)
        //             {
        //                 _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //                     nameof(SeedUsersTenantYBZONEAsync), "User already exists, skipping", new { user.Email });
        //                 continue;
        //             }

        //             // Step 2: Create user in Keycloak
        //             var userRepresentation = UserRepresentation.CreateBuilder()
        //                 .WithUsername(user.Email!)
        //                 .WithEmail(user.Email!)
        //                 .WithEnabled(true)
        //                 .WithFirstName(user.Profile.FirstName)
        //                 .WithLastName(user.Profile.LastName)
        //                 .WithEmailVerified(user.EmailConfirmed)
        //                 .WithPassword(DEFAULT_PASSWORD)
        //                 .WithTenantAttributes(
        //                     tenantId: user.TenantId ?? "",
        //                     subDomain: null,
        //                     tenantType: null,
        //                     branchId: user.BranchId ?? "",
        //                     fullName: $"{user.Profile.FirstName} {user.Profile.LastName}"
        //                 )
        //                 .Build();

        //             string keycloakUserId;
        //             try
        //             {
        //                 keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(userRepresentation);

        //                 _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //                     nameof(_keycloakService.CreateKeycloakUserAsync), "Successfully created user in Keycloak", new { user.Email, keycloakUserId });
        //             }
        //             catch (Exception ex)
        //             {
        //                 _logger.LogError(ex, "::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
        //                     nameof(_keycloakService.CreateKeycloakUserAsync), "Failed to create user in Keycloak", new { user.Email });
        //                 continue; // Skip this user and continue with next
        //             }

        //             // Step 3: Create user in database
        //             var registerCommand = new RegisterCommand
        //             {
        //                 FirstName = user.Profile.FirstName,
        //                 LastName = user.Profile.LastName,
        //                 Email = user.Email!,
        //                 Password = DEFAULT_PASSWORD,
        //                 ConfirmPassword = DEFAULT_PASSWORD,
        //                 PhoneNumber = user.PhoneNumber!,
        //                 BirthDate = user.Profile.BirthDay.ToString("yyyy-MM-dd"),
        //                 Country = user.Profile.Address?.AddressCountry ?? "Vietnam"
        //             };

        //             var createResult = await _identityService.CreateUserAsync(registerCommand, new Guid(keycloakUserId));

        //             if (createResult.IsFailure)
        //             {
        //                 _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
        //                     nameof(_identityService.CreateUserAsync), "Failed to create user in database", new { user.Email, Error = createResult.Error });

        //                 // Cleanup: Delete Keycloak user if database creation fails
        //                 try
        //                 {
        //                     await _keycloakService.DeleteKeycloakUserAsync(keycloakUserId);
        //                 }
        //                 catch (Exception cleanupEx)
        //                 {
        //                     _logger.LogWarning(cleanupEx, "::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
        //                         nameof(_keycloakService.DeleteKeycloakUserAsync), "Failed to cleanup Keycloak user", new { keycloakUserId });
        //                 }
        //                 continue; // Skip this user and continue with next
        //             }

        //             var createdUser = createResult.Response!;

        //             _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //                 nameof(_identityService.CreateUserAsync), "Successfully created user in database", new { user.Email, createdUser.Id });

        //             // Step 4: Update tenant and branch IDs (since CreateUserAsync uses hardcoded defaults)
        //             if (!string.IsNullOrEmpty(user.TenantId) || !string.IsNullOrEmpty(user.BranchId))
        //             {
        //                 createdUser.TenantId = user.TenantId;
        //                 createdUser.BranchId = user.BranchId;
        //                 createdUser.EmailConfirmed = user.EmailConfirmed;

        //                 var updateResult = await _userManager.UpdateAsync(createdUser);
        //                 if (!updateResult.Succeeded)
        //                 {
        //                     _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
        //                         nameof(_userManager.UpdateAsync), "Failed to update tenant/branch IDs", new { user.Email, Errors = updateResult.Errors });
        //                 }
        //                 else
        //                 {
        //                     _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //                         nameof(_userManager.UpdateAsync), "Successfully updated tenant/branch IDs", new { user.Email, TenantId = user.TenantId, BranchId = user.BranchId });
        //                 }
        //             }

        //             // Step 5: Assign roles to user (match by email since user.Id from seed is different from created user's ID)
        //             var rolesForUser = userRoles
        //                 .Where(ur => ur.UserId == user.Id)
        //                 .Select(ur => GetRoleNameById(ur.RoleId))
        //                 .Where(r => !string.IsNullOrEmpty(r))
        //                 .ToList()!;

        //             if (rolesForUser.Any())
        //             {
        //                 var roleAssignmentResult = await _identityService.AssignRolesAsync(createdUser, rolesForUser);

        //                 if (roleAssignmentResult.IsFailure)
        //                 {
        //                     _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
        //                         nameof(_identityService.AssignRolesAsync), "Failed to assign roles to user", new { user.Email, Error = roleAssignmentResult.Error });
        //                 }
        //                 else
        //                 {
        //                     _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //                         nameof(_identityService.AssignRolesAsync), "Successfully assigned roles to user", new { user.Email, Roles = rolesForUser });
        //                 }
        //             }
        //             else
        //             {
        //                 _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
        //                     nameof(SeedUsersTenantYBZONEAsync), "No roles found for user", new { user.Email, UserId = user.Id });
        //             }
        //         }
        //         catch (Exception ex)
        //         {
        //             _logger.LogError(ex, "::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
        //                 nameof(SeedUsersTenantYBZONEAsync), "Unexpected error while seeding user", new { user.Email });
        //             // Continue with next user
        //         }
        //     }

        //     _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
        //         nameof(SeedUsersTenantYBZONEAsync), "Completed seeding YBZONE users", new { UserCount = users.Count });

        //     return true;
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
        //         nameof(SeedUsersTenantYBZONEAsync), ex.Message, new { });
        //     return Result<bool>.Failure(Error.BadRequest(code: "SeedService.Failed", message: "Failed to seed users", serviceName: "IdentityService"));
        // }
    }

    /// <summary>
    /// Gets user roles for YBZONE tenant based on the UserRoles_HCM_TD_KVC_1060 collection
    /// which actually contains YBZONE user role mappings
    /// </summary>
    private static List<IdentityUserRole<string>> GetUserRolesForYBZONE()
    {
        // The UserRoles_HCM_TD_KVC_1060 collection actually contains YBZONE user mappings
        // based on the comments in the code (ADMIN_SUPER_YBZONE, ADMIN_YBZONE)
        return SeedStaffs.UserRoles_YBZONE.ToList();
    }

    /// <summary>
    /// Maps role ID to role name based on SeedData.Roles
    /// </summary>
    private static string? GetRoleNameById(string roleId)
    {
        // Map role IDs to role names based on SeedData
        return roleId switch
        {
            "3a0efb98-7841-4ff1-900c-e255ec60eb4f" => "ADMIN_SUPER",
            "12d826a4-a9c0-471c-91f3-39b18993e0c1" => "ADMIN",
            "12145c29-e918-4cee-b58c-e6fe2a66e560" => "STAFF",
            "11118cf4-b9d1-430d-96c1-4e5272d6d112" => "USER",
            _ => null
        };
    }
}

