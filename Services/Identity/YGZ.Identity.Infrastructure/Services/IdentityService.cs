
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Core.Errors;

namespace YGZ.Identity.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IKeycloakService _keycloakService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IIdentityDbContext _identityDbContext;

    public IdentityService(ILogger<IdentityService> logger,
                           UserManager<User> userManager,
                           IPasswordHasher<User> passwordHasher,
                           IKeycloakService keycloakService,
                           RoleManager<IdentityRole> roleManager,
                           IIdentityDbContext identityDbContext)
    {
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _keycloakService = keycloakService;
        _roleManager = roleManager;
        _identityDbContext = identityDbContext;
    }
    public async Task<Result<User>> FindUserAsync(string email)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is null)
            {
                return Errors.User.DoesNotExist;
            }

            return existingUser;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to find user: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<Result<User>> FindUserAsyncIgnoreFilters(string email)
    {
        try
        {
            // Use IgnoreQueryFilters to bypass tenant filtering for this specific endpoint
            var existingUser = await _identityDbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant());

            if (existingUser is null)
            {
                return Errors.User.DoesNotExist;
            }

            return existingUser;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> CreateUserAsync(RegisterCommand request, Guid userId)
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
                birthDay: DateTime.Parse(request.BirthDay).ToUniversalTime().AddHours(7),
                image: null,
                country: request.Country,
                emailConfirmed: false,
                tenantId: null,
                branchId: null,
                tenantCode: null
            );

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            var checkRoleExist = await _roleManager.RoleExistsAsync(AuthorizationConstants.Roles.USER); 

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create user: {ErrorMessage}", newUser.Email);
                return Errors.User.CannotBeCreated;
            }

            var checkUserRoleExist = await _roleManager.RoleExistsAsync(AuthorizationConstants.Roles.USER);

            if (!checkUserRoleExist)
            {
                _logger.LogError("User role does not exist: {ErrorMessage}", AuthorizationConstants.Roles.USER);
                await _userManager.DeleteAsync(newUser);
                return Errors.User.CannotBeCreated;
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, AuthorizationConstants.Roles.USER);

            if (!roleResult.Succeeded)
            {
                _logger.LogError("Failed to add role to user: {ErrorMessage}", AuthorizationConstants.Roles.USER);
                await _userManager.DeleteAsync(newUser);
                return Errors.User.CannotBeCreated;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to create user: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<Result<bool>> LoginAsync(User user, string password)
    {
        try
        {
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to login: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    public async Task<Result<string>> GenerateEmailVerificationTokenAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

           if(user is null)
           {
            return Errors.User.DoesNotExist;
           }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }
        catch (Exception)
        {
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
                return Errors.Auth.InvalidToken;
            }

            // Verify the token using UserManager
            bool isValid = await _userManager.VerifyUserTokenAsync(
                user,
                "Default", // The provider name (matches AddDefaultTokenProviders)
                UserManager<User>.ConfirmEmailTokenPurpose, // The purpose of the token
                decodedToken);

            if (!isValid)
            {
                return Errors.Auth.InvalidToken;
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!confirmResult.Succeeded)
            {
                return Errors.Auth.ConfirmEmailVerificationFailure;
            }

            return true;
        }
        catch (Exception)
        {
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
                return searchResult.Error;
            }

            var user = searchResult.Response!;
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }
        catch (Exception)
        {
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
                return Errors.Auth.InvalidToken;
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

            if (!resetPasswordResult.Succeeded)
            {
                return Errors.User.CannotResetPassword;
            }

            var resetPasswordKeycloak = await _keycloakService.ResetPasswordAsync(email, newPassword);

            return true;
        }
        catch (Exception)
        {
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
                return Errors.User.InvalidCredentials;
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                return Errors.User.CannotChangePassword;
            }

            var keycloakResult = await _keycloakService.ResetPasswordAsync(user.Email!, newPassword);

            if (keycloakResult.IsFailure)
            {
                return keycloakResult.Error;
            }

            return true;
        }
        catch (Exception)
        {
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
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<User?>> FindUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);

        if(user is null)
        {
            return Errors.User.DoesNotExist;
        }

        return user;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Result<bool>> DeleteUserAsync(string keycloakUserId)
    {
        try
        {
            if (!Guid.TryParse(keycloakUserId, out var userId))
            {
                return Errors.User.DoesNotExist;
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return Errors.User.DoesNotExist;
            }

            var deleteResult = await _userManager.DeleteAsync(user);

            if (!deleteResult.Succeeded)
            {
                return Errors.User.CannotBeDeleted;
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
