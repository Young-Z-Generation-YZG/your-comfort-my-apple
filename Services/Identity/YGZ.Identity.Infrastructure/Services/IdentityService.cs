
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IKeycloakService _keycloakService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private const string DEFAULT_TENANT_ID = "664355f845e56534956be32b";
    private const string DEFAULT_BRANCH_ID = "664357a235e84033bbd0e6b6";

    public IdentityService(ILogger<IdentityService> logger,
                           UserManager<User> userManager,
                           IPasswordHasher<User> passwordHasher,
                           IKeycloakService keycloakService,
                           RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _keycloakService = keycloakService;
        _roleManager = roleManager;
    }
    public async Task<Result<User>> FindUserAsync(string email, bool ignoreBaseFilter = false)
    {
        try
        {
            User? existingUser;

            if (ignoreBaseFilter)
            {
                existingUser = await _userManager.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
            }
            else
            {
                existingUser = await _userManager.FindByEmailAsync(email);
            }

            if (existingUser is null)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.FindByEmailAsync), "User not found", new { email });

                return Errors.User.DoesNotExist;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(FindUserAsync), "Successfully found user", new { email });

            return existingUser;
        }
        catch (Exception ex)
        {
            var parameters = new { email, ignoreBaseFilter };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(FindUserAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<User>> CreateUserAsync(RegisterCommand request, Guid userId)
    {
        try
        {
            var newUser = User.Create(
                guid: userId,
                email: request.Email,
                phoneNumber: request.PhoneNumber,
                passwordHash: "",
                firstName: request.FirstName,
                lastName: request.LastName,
                birthDay: DateTime.Parse(request.BirthDate).ToUniversalTime().AddHours(7),
                image: null,
                country: request.Country,
                emailConfirmed: false,
                tenantId: DEFAULT_TENANT_ID,
                branchId: DEFAULT_BRANCH_ID
            );

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.CreateAsync), "Failed to create user in database", result.Errors);

                return Errors.User.CannotBeCreated;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(CreateUserAsync), "Successfully created user in database", new { request.Email, userId });

            return newUser;
        }
        catch (Exception ex)
        {
            var parameters = new { email = request.Email, userId };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(CreateUserAsync), ex.Message, parameters);

            throw;
        }
    }

    public async Task<Result<bool>> AssignRolesAsync(User user, List<string> roleNames)
    {
        try
        {
            if (roleNames == null || roleNames.Count == 0)
            {
                _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(AssignRolesAsync), "No roles provided for assignment to user", new { user.Email });
                
                return Errors.Keycloak.CannotAssignRole;
            }

            // Check if all roles exist
            foreach (var roleName in roleNames)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                        nameof(_roleManager.RoleExistsAsync), "Role does not exist", new { roleName });
                    
                    return Errors.Keycloak.RoleNotFound;
                }
            }

            // Assign all roles to user
            var roleResult = await _userManager.AddToRolesAsync(user, roleNames);

            if (!roleResult.Succeeded)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.AddToRolesAsync), "Failed to add roles to user", new { roleNames, user.Email, roleResult.Errors });

                return Errors.Keycloak.CannotAssignRole;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(AssignRolesAsync), "Successfully assigned roles to user", new { roleNames, user.Email });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { userId = user.Id, email = user.Email, roleNames };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(AssignRolesAsync), ex.Message, parameters);
            throw;
        }
    }

    public Result<bool> Login(User user, string password)
    {
        try
        {
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_passwordHasher.VerifyHashedPassword), "Invalid credentials", new { user.Id, user.Email });

                return Errors.User.InvalidCredentials;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Login), "Successfully logged in", new { user.Id, user.Email });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { userId = user.Id, email = user.Email };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Login), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<string>> GenerateEmailVerificationTokenAsync(User user)
    {
        try
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (token is null)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.GenerateEmailConfirmationTokenAsync), "Failed to generate email verification token", new { user.Id, user.Email });

                return Errors.Auth.InvalidToken;
            }

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            if (encodedToken is null)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.GenerateEmailConfirmationTokenAsync), "Failed to encode email verification token", new { user.Id, user.Email });

                return Errors.Auth.InvalidToken;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(GenerateEmailVerificationTokenAsync), "Successfully encoded email verification token", new { user.Id, user.Email });

            return encodedToken;
        }
        catch (Exception ex)
        {
            var parameters = new { userId = user.Id, email = user.Email };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GenerateEmailVerificationTokenAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<bool>> VerifyEmailTokenAsync(string email, string encodedToken)
    {
        try
        {
            // Find the user
            var searchResult = await FindUserAsync(email);

            if (searchResult.IsFailure)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(FindUserAsync), "User not found", new { email });

                return searchResult.Error;
            }

            var user = searchResult.Response!;

            // Decode the Base64 token
            string decodedToken;

            try
            {
                byte[] tokenBytes = Convert.FromBase64String(encodedToken);
                decodedToken = Encoding.UTF8.GetString(tokenBytes);
            }
            catch (FormatException)
            {
                throw new FormatException("Failed to decode token");
            }

            // Verify the token using UserManager
            bool isValid = await _userManager.VerifyUserTokenAsync(
                user,
                "Default", // The provider name (matches AddDefaultTokenProviders)
                UserManager<User>.ConfirmEmailTokenPurpose, // The purpose of the token
                decodedToken);

            if (!isValid)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.VerifyUserTokenAsync), "Invalid token", new { user.Id, user.Email });

                return Errors.Auth.InvalidToken;
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!confirmResult.Succeeded)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.ConfirmEmailAsync), "Failed to confirm email verification", new { user.Id, user.Email });

                return Errors.Auth.ConfirmEmailVerificationFailure;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(VerifyEmailTokenAsync), "Successfully verified email token", new { user.Id, user.Email });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { email, hasToken = !string.IsNullOrEmpty(encodedToken) };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(VerifyEmailTokenAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<string>> GenerateResetPasswordTokenAsync(string email)
    {
        try
        {
            var searchResult = await FindUserAsync(new(email));

            if (searchResult.IsFailure)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(FindUserAsync), "User not found", new { email });

                return searchResult.Error;
            }

            var user = searchResult.Response!;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            if (encodedToken is null)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.GeneratePasswordResetTokenAsync), "Failed to encode reset password token", new { user.Id, user.Email });

                return Errors.Auth.InvalidToken;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(GenerateResetPasswordTokenAsync), "Successfully encoded reset password token", new { user.Id, user.Email });

            return encodedToken;
        }
        catch (Exception ex)
        {
            var parameters = new { email };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GenerateResetPasswordTokenAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<bool>> VerifyResetPasswordTokenAsync(string email, string encodedToken, string newPassword)
    {
        try
        {
            // Find the user
            var searchResult = await FindUserAsync(email);

            if (searchResult.IsFailure)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(FindUserAsync), "User not found", new { email });

                return searchResult.Error;
            }

            var user = searchResult.Response!;

            // Decode the Base64 token
            string decodedToken;

            try
            {
                byte[] tokenBytes = Convert.FromBase64String(encodedToken);
                decodedToken = Encoding.UTF8.GetString(tokenBytes);
            }
            catch (FormatException)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Convert.FromBase64String), "Failed to decode token", new { encodedToken });

                return Errors.Auth.InvalidToken;
            }

            // Verify the token using UserManager
            bool isValid = await _userManager.VerifyUserTokenAsync(
                user,
                "Default", // The provider name (matches AddDefaultTokenProviders)
                UserManager<User>.ResetPasswordTokenPurpose, // The purpose of the token
                decodedToken);

            if (!isValid)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.VerifyUserTokenAsync), "Invalid token", new { user.Id, user.Email });

                return Errors.Auth.InvalidToken;
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (!resetPasswordResult.Succeeded)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.ResetPasswordAsync), "Failed to reset password", new { user.Id, user.Email });

                return Errors.User.CannotResetPassword;
            }

            var resetPasswordKeycloak = await _keycloakService.ResetPasswordAsync(email, newPassword);

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(VerifyResetPasswordTokenAsync), "Successfully verified reset password token", new { user.Id, user.Email });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { email, hasToken = !string.IsNullOrEmpty(encodedToken), hasNewPassword = !string.IsNullOrEmpty(newPassword) };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(VerifyResetPasswordTokenAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<bool>> ChangePasswordAsync(User user, string oldPassword, string newPassword)
    {
        try
        {
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, oldPassword);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_passwordHasher.VerifyHashedPassword), "Invalid credentials", new { user.Id, user.Email });

                return Errors.User.InvalidCredentials;
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.ChangePasswordAsync), "Failed to change password", new { user.Id, user.Email });

                return Errors.User.CannotChangePassword;
            }

            var keycloakResult = await _keycloakService.ResetPasswordAsync(user.Email!, newPassword);

            if (keycloakResult.IsFailure)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_keycloakService.ResetPasswordAsync), "Failed to reset password", new { user.Email });

                return keycloakResult.Error;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(ChangePasswordAsync), "Successfully changed password", new { user.Id, user.Email });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { userId = user.Id, email = user.Email };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(ChangePasswordAsync), ex.Message, parameters);
            return Errors.User.CannotChangePassword;
        }
    }

    public async Task<Result<List<string>>> GetRolesAsync(User user)
    {
        try
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        catch (Exception ex)
        {
            var parameters = new { userId = user.Id, email = user.Email };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GetRolesAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<User?>> FindUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }
        catch (Exception ex)
        {
            var parameters = new { email };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(FindUserByEmailAsync), ex.Message, parameters);
            throw;
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(string keycloakUserId)
    {
        try
        {
            if (!Guid.TryParse(keycloakUserId, out var userId))
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(Guid.TryParse), "Invalid user ID", new { keycloakUserId });

                return Errors.User.DoesNotExist;
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.FindByIdAsync), "User not found", new { userId });

                return Errors.User.DoesNotExist;
            }

            var deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                _logger.LogError("::[Operation Error]:: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                    nameof(_userManager.DeleteAsync), "Failed to delete user", new { user.Id, user.Email });

                return Errors.User.CannotBeDeleted;
            }

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(DeleteUserAsync), "Successfully deleted user", new { user.Id, user.Email });

            return true;
        }
        catch (Exception ex)
        {
            var parameters = new { keycloakUserId };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(DeleteUserAsync), ex.Message, parameters);
            throw;
        }
    }
}
