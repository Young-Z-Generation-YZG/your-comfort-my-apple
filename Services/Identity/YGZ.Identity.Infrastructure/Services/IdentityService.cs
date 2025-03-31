
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Emails;
using YGZ.Identity.Application.Abstractions.Services;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Application.Auths.Extensions;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<IdentityService> _logger;
    private readonly IKeycloakService _keycloakService;
    private readonly IEmailService _emailService;

    public IdentityService(
        ILogger<IdentityService> logger,
        UserManager<User> userManager,
        IPasswordHasher<User> passwordHasher,
        IKeycloakService keycloakService,
        IEmailService emailService)
    {
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _keycloakService = keycloakService;
        _emailService = emailService;
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
            _logger.LogError(ex, ex.Message, nameof(FindUserAsync));
            throw;
        }
    }

    public async Task<Result<bool>> CreateUserAsync(RegisterCommand request, Guid userId)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return Errors.User.AlreadyExists;
            }

            var newUser = request.ToEntity(userId);

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                return Errors.User.CannotBeCreated;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(CreateUserAsync));
            throw;
        }
    }

    public async Task<Result<User>> LoginAsync(LoginCommand request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is null)
            {
                return Errors.User.DoesNotExist;
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash!, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return Errors.User.InvalidCredentials;
            }

            return existingUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(LoginAsync));
            throw;
        }
    }

    public async Task<Result<string>> GenerateEmailVerificationTokenAsync(string email)
    {
        try
        {
            var searchResult = await FindUserAsync(email);
            if (searchResult.IsFailure)
            {
                return searchResult.Error;
            }

            var user = searchResult.Response!;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return encodedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate email verification token for {Email}", email);
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
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Invalid Base64 token format for email {Email}", email);
                return Errors.Auth.InvalidToken;
            }

            // Verify the token using UserManager
            bool isValid = await _userManager.VerifyUserTokenAsync(
                user,
                "Default", // The provider name (matches AddDefaultTokenProviders)
                "EmailConfirmation", // The purpose of the token
                decodedToken);

            if (!isValid)
            {
                return Errors.Auth.InvalidToken;
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!confirmResult.Succeeded)
            {
                _logger.LogWarning("Failed to confirm email for {Email}: {Errors}",
                    email, string.Join(", ", confirmResult.Errors.Select(e => e.Description)));

                return Errors.Auth.ConfirmEmailVerificationFailure;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify email token for {Email}", email);
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

            var token = await _userManager
                .GeneratePasswordResetTokenAsync(searchResult.Response!)
                .ConfigureAwait(false);

            var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GenerateResetPasswordTokenAsync));
            throw;
        }
    }

    public async Task<Result<bool>> CheckTokenIsValid(string email, string encodedToken)
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
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Invalid Base64 token format for email {Email}", email);
                return Errors.Auth.InvalidToken;
            }

            // Verify the token using UserManager
            bool isValid = await _userManager.VerifyUserTokenAsync(
                user,
                "Default", // The provider name (matches AddDefaultTokenProviders)
                "EmailConfirmation", // The purpose of the token
                decodedToken);

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify email token for {Email}", email);
            throw;
        }
    }
}
