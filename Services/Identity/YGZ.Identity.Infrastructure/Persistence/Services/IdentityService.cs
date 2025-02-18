
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;
using YGZ.Identity.Application.Auths.Extensions;
using YGZ.Identity.Domain.Core.Errors;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Services;

public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IPasswordHasher<User> _passwordHasher;

    public IdentityService(ILogger<IdentityService> logger,
                           UserManager<User> userManager,
                           IPasswordHasher<User> passwordHasher,
                           RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _passwordHasher = passwordHasher;
        _roleManager = roleManager;
    }

    public async Task<Result<User>> FindUserAsync(string email)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if(existingUser is null)
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

    public async Task<Result<bool>> CreateUserAsync(RegisterCommand request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return Errors.User.AlreadyExists;
            }

            var newUser = request.ToEntity();

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

            var passwordIsValid = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash!, request.Password);

            if (passwordIsValid == PasswordVerificationResult.Failed)
            {
                return Errors.User.InvalidPassword;
            }

            return existingUser;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(LoginAsync));
            throw;
        }   
    }

    public async Task<Result<string>> GenerateResetPasswordTokenAsync(string email)
    {
        try
        {
            var existingUser = await FindUserAsync(email);

            if (existingUser is null)
            {
                return Errors.User.DoesNotExist;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser.Response!);

            var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GenerateResetPasswordTokenAsync));
            throw;
        }
    }

    public async Task<Result<string>> GenerateEmailVerificationTokenAsync(string email)
    {
        try
        {
            var existingUser = await FindUserAsync(email);

            if(existingUser is null)
            {
                return Errors.User.DoesNotExist;
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser.Response!);

            var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, nameof(GenerateEmailVerificationTokenAsync));
            throw;
        }
    }

    public List<IdentityRole> FindAllRoles()
    {
        var roles = _roleManager.Roles.ToList();

        return roles;
    }

    public List<IdentityRole> GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> CreateRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<Result<User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<IdentityRole>>> GetUserRolesAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> RemoveUserFromRoleAsync(string email, string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<Claim>>> GetAllClaims(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> AddClaimToUserAsync(string email, string claimName, string claimValue)
    {
        throw new NotImplementedException();
    }
}
