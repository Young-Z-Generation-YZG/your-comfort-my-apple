
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Text;
using YGZ.Identity.Application.Core.Abstractions.Identity;
using YGZ.Identity.Application.Identity.Commands.CreateUser;
using YGZ.Identity.Application.Identity.Common.Dtos;
using YGZ.Identity.Application.Identity.Extensions;
using YGZ.Identity.Domain.Common.Abstractions;
using YGZ.Identity.Domain.Common.Errors;
using YGZ.Identity.Domain.Identity.Entities;

namespace YGZ.Identity.Persistence.Services.Identity;

public sealed class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly SignInManager<User> _signInManager;

    public IdentityService(
        ILogger<IdentityService> logger,
        UserManager<User> userManager,
        IPasswordHasher<User> passwordHasher,
        SignInManager<User> signInManager
    )
    {
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _signInManager = signInManager;
    }

    public async Task<Result<bool>> CreateUserAsync(CreateUserCommand request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);


            if (user is not null)
                return Errors.Identity.UserAlreadyExists;

            user = request.ToEntity();

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            var result = await _userManager.CreateAsync(user).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return Errors.Identity.UserCannotBeCreated;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(CreateUserAsync));

            throw;
        }
    }

    public async Task<Result<User>> FindUserAsync(FindUserDto request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

            if (user is null)
                return Errors.Identity.UserDoesNotExist;

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(FindUserAsync));

            throw;
        }
    }

    public async Task<Result<string>> GenerateEmailVerificationTokenAsync(string email)
    {
        try
        {
            var searchResult = await FindUserAsync(new(email));

            if (searchResult.IsFailure)
                return searchResult.Error;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(searchResult.Response!).ConfigureAwait(false);

            var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(FindUserAsync));

            throw;
        }
    }

    public Task<Result<string>> GenerateResetPasswordTokenAsync(string email)
    {
        throw new NotImplementedException();
    }
}
