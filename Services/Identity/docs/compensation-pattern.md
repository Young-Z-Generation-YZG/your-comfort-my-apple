# Compensation Pattern for User Registration

## Overview

The Identity service uses a **Compensation Pattern** (also known as Saga Pattern) to handle rollback operations in distributed transactions. This pattern is essential when coordinating operations across multiple systems (Keycloak, Database, Cache, Email Service).

## Problem Statement

User registration involves multiple steps:
1. Create user in Keycloak
2. Create user in Database
3. Generate and cache OTP
4. Generate email verification token
5. Send verification email

If any step fails, we need to rollback all previous steps to maintain data consistency.

## Solution: Compensation Pattern

Instead of using traditional ACID transactions (which don't work across distributed systems), we use **compensating actions** that undo the effects of completed steps.

### Key Components

#### 1. `IUserRegistrationCompensator`
- **Purpose**: Tracks registration steps and performs rollback when needed
- **Location**: `Application/Abstractions/Services/IUserRegistrationCompensator.cs`
- **Key Methods**:
  - `TrackStep()`: Records each completed step
  - `CompensateAsync()`: Performs rollback in reverse order
  - `GetState()`: Returns current registration state

#### 2. `UserRegistrationCompensator`
- **Purpose**: Implementation of compensation logic
- **Location**: `Infrastructure/Services/UserRegistrationCompensator.cs`
- **Compensation Order** (reverse of creation):
  1. Email sent → No rollback (idempotent)
  2. OTP cached → Expires naturally (no explicit cleanup needed)
  3. Email token generated → Expires naturally
  4. Database user → Delete from database
  5. Keycloak user → Delete from Keycloak

## Usage Example

```csharp
public class RegisterHandler : ICommandHandler<RegisterCommand, EmailVerificationResponse>
{
    private readonly IUserRegistrationCompensator _compensator;

    public async Task<Result<EmailVerificationResponse>> Handle(...)
    {
        try
        {
            // Step 1: Create Keycloak user
            var keycloakUserId = await _keycloakService.CreateKeycloakUserAsync(...);
            _compensator.TrackStep(RegistrationStep.KeycloakUserCreated, keycloakUserId);

            // Step 2: Create database user
            var result = await _identityService.CreateUserAsync(...);
            if (result.IsFailure)
            {
                await _compensator.CompensateAsync(keycloakUserId, null);
                return Errors.User.FailedToRegister;
            }
            _compensator.TrackStep(RegistrationStep.DatabaseUserCreated, keycloakUserId);

            // ... more steps ...

            return success;
        }
        catch (Exception ex)
        {
            // Automatic compensation on unexpected errors
            var state = _compensator.GetState();
            await _compensator.CompensateAsync(state.KeycloakUserId, state.DatabaseUserId);
            return Errors.User.FailedToRegister;
        }
    }
}
```

## Best Practices

### 1. **Track Steps Immediately After Success**
Always track a step immediately after it succeeds, before proceeding to the next step.

```csharp
// ✅ Good
var userId = await CreateUser();
_compensator.TrackStep(RegistrationStep.UserCreated, userId);
var nextResult = await NextStep();

// ❌ Bad
var userId = await CreateUser();
var nextResult = await NextStep();
_compensator.TrackStep(RegistrationStep.UserCreated, userId); // Too late!
```

### 2. **Compensate on Every Failure Point**
Compensate at every point where a step might fail.

```csharp
var result = await SomeOperation();
if (result.IsFailure)
{
    await _compensator.CompensateAsync(...); // Compensate before returning
    return result.Error;
}
```

### 3. **Compensate in Reverse Order**
The compensator automatically handles reverse-order rollback. This ensures dependencies are cleaned up correctly.

### 4. **Handle Compensation Failures Gracefully**
Compensation operations themselves might fail. The compensator logs these but doesn't throw, allowing partial cleanup.

### 5. **Idempotent Operations**
Operations like email sending are idempotent - they can be safely retried without side effects. These don't need explicit rollback.

### 6. **Use Try-Catch for Unexpected Errors**
Wrap the entire operation in try-catch to handle unexpected exceptions and trigger compensation.

## Advantages

1. **Centralized Rollback Logic**: All compensation logic is in one place
2. **Reusable**: Can be used across multiple handlers (Register, AddNewStaff, etc.)
3. **Maintainable**: Easy to add/remove steps
4. **Testable**: Compensation logic can be unit tested independently
5. **Observable**: State tracking allows for better logging and debugging

## Limitations

1. **Eventual Consistency**: Some cleanup operations might fail, requiring manual intervention
2. **No Atomic Guarantees**: Unlike ACID transactions, compensation doesn't guarantee atomicity
3. **Partial Failures**: If compensation itself fails, manual cleanup may be needed

## Alternative Patterns

### 1. **Outbox Pattern**
For eventual consistency, consider using an outbox pattern where operations are recorded and processed asynchronously.

### 2. **Two-Phase Commit (2PC)**
Not recommended for distributed systems due to performance and availability issues.

### 3. **Event Sourcing**
Can help with rollback by replaying events, but adds complexity.

## Monitoring & Alerts

Monitor compensation operations:
- Log all compensation attempts
- Alert on compensation failures
- Track compensation success rates
- Monitor orphaned resources (users created but not fully registered)

## Future Enhancements

1. **Compensation Retry**: Add retry logic for failed compensation operations
2. **Compensation Queue**: Queue compensation operations for async processing
3. **Compensation Audit**: Track all compensation operations for audit purposes
4. **Automatic Cleanup Jobs**: Background jobs to clean up orphaned resources
