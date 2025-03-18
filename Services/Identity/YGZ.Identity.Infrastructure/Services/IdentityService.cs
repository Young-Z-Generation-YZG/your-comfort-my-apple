
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
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

    public IdentityService(
        ILogger<IdentityService> logger,
        UserManager<User> userManager,
        IPasswordHasher<User> passwordHasher,
        IKeycloakService keycloakService)
    {
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _keycloakService = keycloakService;
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
}
