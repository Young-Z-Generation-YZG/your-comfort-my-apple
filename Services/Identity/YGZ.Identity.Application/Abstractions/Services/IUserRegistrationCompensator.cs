using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Identity.Application.Abstractions.Services;

/// <summary>
/// Service responsible for compensating (rolling back) user registration operations
/// when failures occur during the distributed transaction.
/// </summary>
public interface IUserRegistrationCompensator
{
    /// <summary>
    /// Compensates for user registration by rolling back all created resources.
    /// </summary>
    /// <param name="keycloakUserId">The Keycloak user ID to rollback (if created)</param>
    /// <param name="databaseUserId">The database user ID to rollback (if created)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result indicating success or failure of compensation</returns>
    Task<Result<bool>> CompensateAsync(
        string? keycloakUserId = null,
        string? databaseUserId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Tracks a step in the registration process for potential rollback.
    /// </summary>
    /// <param name="step">The step to track</param>
    /// <param name="resourceId">The resource ID to track (optional)</param>
    void TrackStep(RegistrationStep step, string? resourceId = null);

    /// <summary>
    /// Gets the current state of registration steps.
    /// </summary>
    RegistrationState GetState();
}

/// <summary>
/// Represents a step in the user registration process.
/// </summary>
public enum RegistrationStep
{
    None = 0,
    KeycloakUserCreated = 1,
    DatabaseUserCreated = 2,
    RoleAssigned = 3,
    EmailTokenGenerated = 4,
    OtpCached = 5,
    EmailSent = 6
}

/// <summary>
/// Represents the current state of registration steps.
/// </summary>
public class RegistrationState
{
    public string? KeycloakUserId { get; set; }
    public string? DatabaseUserId { get; set; }
    public bool RoleAssigned { get; set; }
    public bool EmailTokenGenerated { get; set; }
    public bool OtpCached { get; set; }
    public bool EmailSent { get; set; }
    public RegistrationStep LastCompletedStep { get; set; } = RegistrationStep.None;
}
