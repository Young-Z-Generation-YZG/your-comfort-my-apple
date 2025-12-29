using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Identity.Application.Abstractions.Data;
using YGZ.Identity.Application.Abstractions.Services;

namespace YGZ.Identity.Infrastructure.Services;

/// <summary>
/// Implementation of user registration compensation service.
/// Handles rollback of distributed transaction steps when registration fails.
/// </summary>
public class UserRegistrationCompensator : IUserRegistrationCompensator
{
    private readonly ILogger<UserRegistrationCompensator> _logger;
    private readonly IKeycloakService _keycloakService;
    private readonly IIdentityService _identityService;
    private readonly ICachedRepository _cachedRepository;
    private readonly RegistrationState _state;

    public UserRegistrationCompensator(
        ILogger<UserRegistrationCompensator> logger,
        IKeycloakService keycloakService,
        IIdentityService identityService,
        ICachedRepository cachedRepository)
    {
        _logger = logger;
        _keycloakService = keycloakService;
        _identityService = identityService;
        _cachedRepository = cachedRepository;
        _state = new RegistrationState();
    }

    public void TrackStep(RegistrationStep step, string resourceId)
    {
        switch (step)
        {
            case RegistrationStep.KeycloakUserCreated:
                _state.KeycloakUserId = resourceId;
                _state.LastCompletedStep = RegistrationStep.KeycloakUserCreated;
                break;
            case RegistrationStep.DatabaseUserCreated:
                _state.DatabaseUserId = resourceId;
                _state.LastCompletedStep = RegistrationStep.DatabaseUserCreated;
                break;
            case RegistrationStep.RoleAssigned:
                _state.RoleAssigned = true;
                _state.LastCompletedStep = RegistrationStep.RoleAssigned;
                break;
            case RegistrationStep.EmailTokenGenerated:
                _state.EmailTokenGenerated = true;
                _state.LastCompletedStep = RegistrationStep.EmailTokenGenerated;
                break;
            case RegistrationStep.OtpCached:
                _state.OtpCached = true;
                _state.LastCompletedStep = RegistrationStep.OtpCached;
                break;
            case RegistrationStep.EmailSent:
                _state.EmailSent = true;
                _state.LastCompletedStep = RegistrationStep.EmailSent;
                break;
        }
    }

    public RegistrationState GetState() => _state;

    public async Task<Result<bool>> CompensateAsync(
        string? keycloakUserId = null,
        string? databaseUserId = null,
        CancellationToken cancellationToken = default)
    {
        // Use provided IDs or fall back to tracked state
        var kcUserId = keycloakUserId ?? _state.KeycloakUserId;
        var dbUserId = databaseUserId ?? _state.DatabaseUserId;

        var compensationErrors = new List<string>();

        // Compensate in reverse order of creation
        // Step 5: Email sent - No rollback needed (idempotent)
        if (_state.EmailSent)
        {
            _logger.LogInformation("Email was sent, but this is idempotent - no rollback needed");
        }

        // Step 4: OTP cached - Remove from cache
        if (_state.OtpCached && !string.IsNullOrEmpty(dbUserId))
        {
            try
            {
                // Note: We'd need email to remove OTP, but this is best-effort cleanup
                _logger.LogInformation("OTP was cached, but cache will expire automatically");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to remove OTP from cache during compensation");
                compensationErrors.Add($"OTP cache cleanup: {ex.Message}");
            }
        }

        // Step 3: Email token generated - No rollback needed (tokens expire naturally)

        // Step 2: Database user created - Delete from database
        if (!string.IsNullOrEmpty(dbUserId))
        {
            try
            {
                var deleteResult = await _identityService.DeleteUserAsync(dbUserId);
                if (deleteResult.IsFailure)
                {
                    _logger.LogWarning("Failed to delete database user during compensation: {UserId}", dbUserId);
                    compensationErrors.Add($"Database user deletion: {deleteResult.Error.Message}");
                }
                else
                {
                    _logger.LogInformation("Successfully compensated database user: {UserId}", dbUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting database user during compensation: {UserId}", dbUserId);
                compensationErrors.Add($"Database user deletion exception: {ex.Message}");
            }
        }

        // Step 1: Keycloak user created - Delete from Keycloak
        if (!string.IsNullOrEmpty(kcUserId))
        {
            try
            {
                var deleteResult = await _keycloakService.DeleteKeycloakUserAsync(kcUserId);
                if (deleteResult.IsFailure)
                {
                    _logger.LogWarning("Failed to delete Keycloak user during compensation: {UserId}", kcUserId);
                    compensationErrors.Add($"Keycloak user deletion: {deleteResult.Error.Message}");
                }
                else
                {
                    _logger.LogInformation("Successfully compensated Keycloak user: {UserId}", kcUserId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while deleting Keycloak user during compensation: {UserId}", kcUserId);
                compensationErrors.Add($"Keycloak user deletion exception: {ex.Message}");
            }
        }

        // Log compensation summary
        if (compensationErrors.Any())
        {
            _logger.LogWarning(
                "Compensation completed with {ErrorCount} errors. Errors: {Errors}",
                compensationErrors.Count,
                string.Join("; ", compensationErrors));
            
            // Still return success as we attempted compensation
            // The errors are logged for manual intervention if needed
            return true;
        }

        _logger.LogInformation("Compensation completed successfully for Keycloak: {KcUserId}, Database: {DbUserId}",
            kcUserId ?? "none", dbUserId ?? "none");

        return true;
    }
}
