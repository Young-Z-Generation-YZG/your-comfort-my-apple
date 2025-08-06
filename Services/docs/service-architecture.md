# Service Architecture Documentation

## Overview

This document outlines the standardized microservices architecture pattern used across the YGZ system. The **Identity service** serves as the foundational template that demonstrates best practices and architectural patterns that all other services should follow.

## Sample Service (Base Service) for Others

- **Sample service**: `/Identity`
- **Purpose**: Provides the architectural blueprint for all microservices in the system

## Architecture Principles

### 🏗️ Clean Architecture (4-Layer Pattern)

All services follow the Clean Architecture pattern with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│              API Layer                  │
│    (Controllers, Middleware, HTTP)      │
├─────────────────────────────────────────┤
│           Application Layer             │
│     (CQRS, Business Logic, DTOs)        │
├─────────────────────────────────────────┤
│            Domain Layer                 │
│   (Entities, Value Objects, Events)     │
├─────────────────────────────────────────┤
│          Infrastructure Layer           │
│  (Database, External Services, Cache)   │
└─────────────────────────────────────────┘
```

### 📁 Standard Project Structure

Each service follows this naming convention and structure:

```
ServiceName/
├── YGZ.ServiceName.Api/           # Presentation Layer
│   ├── Controllers/
│   ├── Contracts/
│   ├── Mappings/
│   ├── Extensions/
│   └── Program.cs
├── YGZ.ServiceName.Application/   # Application Layer
│   ├── Features/
│   │   ├── Commands/
│   │   └── Queries/
│   ├── Abstractions/
│   └── DependencyInjection.cs
├── YGZ.ServiceName.Domain/        # Domain Layer
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   └── Core/
└── YGZ.ServiceName.Infrastructure/ # Infrastructure Layer
    ├── Persistence/
    ├── Services/
    ├── Settings/
    └── DependencyInjection.cs
```

## Layer Responsibilities

### 🌐 API Layer (Presentation)

- **Purpose**: Handle HTTP requests and responses
- **Components**:
  - Controllers with API endpoints
  - Request/Response contracts (DTOs)
  - Swagger/OpenAPI documentation
  - Authentication/Authorization middleware
  - Global exception handling
  - Health checks

**Example from Identity service:**

```csharp
services
    .AddPresentationLayer(builder)
    .AddInfrastructureLayer(configuration)
    .AddApplicationLayer(configuration);

// Additional common services
services.AddProblemDetails();           // RFC 7807 error responses
services.AddHttpContextAccessor();     // Access HttpContext in services
services.AddEndpointsApiExplorer();     // API endpoint discovery
services.ConfigureHttpClientDefaults(http =>
    http.AddStandardResilienceHandler()); // HTTP resilience patterns
```

### 🔧 **Common Service Registrations Explained**

#### `services.AddProblemDetails()`

- **Purpose**: Implements RFC 7807 Problem Details for HTTP APIs
- **What it does**:
  - Standardizes error response format across all endpoints
  - Converts exceptions to structured JSON error responses
  - Provides consistent error handling for clients
- **Example Response**:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "The request is invalid",
  "traceId": "0HMVH8H2M7QAK:00000001"
}
```

#### `services.AddHttpContextAccessor()`

- **Purpose**: Provides access to HttpContext in non-controller classes
- **What it does**:
  - Enables dependency injection of `IHttpContextAccessor`
  - Allows access to current HTTP request context in services
  - Essential for accessing user claims, headers, and request data
- **Usage Example**:

```csharp
public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
```

#### `services.AddEndpointsApiExplorer()`

- **Purpose**: Enables API endpoint discovery for documentation
- **What it does**:
  - Discovers all API endpoints in the application
  - Provides metadata for Swagger/OpenAPI generation
  - Required for automatic API documentation

#### `services.ConfigureHttpClientDefaults()`

- **Purpose**: Configures default HTTP client behavior
- **What it does**:
  - Adds resilience patterns (retry, circuit breaker, timeout)
  - Configures default headers and policies
  - Ensures consistent HTTP client behavior across services
- **Resilience Patterns**:
  - **Retry**: Automatic retry on transient failures
  - **Circuit Breaker**: Prevents cascade failures
  - **Timeout**: Request timeout handling

### 🎯 Application Layer

- **Purpose**: Orchestrate business workflows and use cases
- **Components**:
  - CQRS Commands and Queries (MediatR)
  - Command/Query Handlers
  - FluentValidation validators
  - Application service interfaces
  - DTOs and mapping profiles

**Key Patterns:**

- Command Query Responsibility Segregation (CQRS)
- Mediator pattern for decoupling
- Pipeline behaviors for cross-cutting concerns

#### **CQRS Implementation Example**

```csharp
// Command (Write Operation)
public record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : ICommand<Result<Guid>>;

// Command Handler
public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IIdentityService _identityService;

    public RegisterUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request, Guid.NewGuid());
        return result.IsSuccess ? Result<Guid>.Success(Guid.NewGuid()) : Result<Guid>.Failure(result.Error);
    }
}

// Query (Read Operation)
public record GetUserQuery(string Email) : IQuery<Result<UserResponse>>;

// Query Handler
public class GetUserQueryHandler : IQueryHandler<GetUserQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        return user != null ? Result<UserResponse>.Success(user.ToResponse()) : Result<UserResponse>.Failure(UserErrors.NotFound);
    }
}
```

#### **FluentValidation Pipeline**

```csharp
// Validator
public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
    }
}

// Validation Pipeline Behavior (Automatic)
public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var errors = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToArray();

            if (errors.Any())
            {
                return CreateValidationResult<TResponse>(errors);
            }
        }

        return await next();
    }
}
```

### 🏛️ Domain Layer

- **Purpose**: Core business logic and rules
- **Components**:
  - Domain entities and aggregate roots
  - Value objects
  - Domain events
  - Business rule validation
  - Domain services (when needed)

**DDD Patterns Used:**

```csharp
// Base Entity with Domain Events
public abstract class Entity<TId> : IEquatable<Entity<TId>>, IAggregate
{
    public TId Id { get; protected set; }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
```

### 🔧 Infrastructure Layer

- **Purpose**: External concerns and technical implementation
- **Components**:
  - Database contexts and repositories
  - External service integrations
  - Configuration settings
  - Caching implementations
  - Message queues
  - File storage

## Shared Building Blocks

### 📦 YGZ.BuildingBlocks.Shared

Contains common patterns and utilities used across all services:

- **Result Pattern**: Consistent error handling
- **CQRS Abstractions**: Base interfaces for commands/queries
- **Validation Behaviors**: FluentValidation pipeline
- **Domain Primitives**: Base entity, value object, aggregate root
- **Extensions**: Common service registrations

### 📨 YGZ.BuildingBlocks.Messaging

- Integration events for inter-service communication
- MassTransit configuration
- Event publishing/subscribing patterns

## Technology Stack Standards

### 🗄️ Databases

- **Primary**: PostgreSQL with Entity Framework Core
- **Caching**: Redis for distributed caching
- **Alternative**: MongoDB for document storage (Catalog service)
- **Event Store**: Marten for event sourcing (Basket service)

### 🔐 Authentication & Authorization

- **Identity Provider**: Keycloak
- **Protocol**: OAuth2/OpenID Connect
- **JWT**: Bearer token authentication
- **Authorization**: Role-based and policy-based

### 📊 Monitoring & Observability

- **Logging**: Serilog with structured logging
- **Tracing**: OpenTelemetry
- **Health Checks**: ASP.NET Core health checks
- **Metrics**: Application performance monitoring

### 🚀 API Standards

- **Documentation**: Swagger/OpenAPI 3.0
- **Versioning**: API versioning support
- **Serialization**: System.Text.Json with snake_case
- **Validation**: FluentValidation
- **Error Handling**: Problem Details (RFC 7807)

## Service Implementation Examples

### ✅ Services Following the Pattern

| Service      | Database            | Special Features                      |
| ------------ | ------------------- | ------------------------------------- |
| **Identity** | PostgreSQL          | User management, Keycloak integration |
| **Basket**   | PostgreSQL + Marten | Event sourcing, Redis caching         |
| **Catalog**  | MongoDB             | Product catalog, image upload         |
| **Ordering** | PostgreSQL          | Order processing, payment integration |
| **Discount** | PostgreSQL          | Coupon management, gRPC services      |

### 🔄 Common Integration Patterns

#### Dependency Injection Registration

```csharp
// In each service's Program.cs
services
    .AddPresentationLayer(builder)      // API layer setup
    .AddInfrastructureLayer(configuration) // Infrastructure setup
    .AddApplicationLayer(configuration);    // Application setup
```

#### Health Checks

```csharp
services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "ServiceDb")
    .AddRedis(redisConnectionString);
```

#### Authentication Setup

```csharp
services.AddKeycloakIdentityServerExtension(configuration);
services.AddKeycloakOpenTelemetryExtensions();
```

### 🔄 **Middleware Pipeline Configuration**

The middleware pipeline is configured consistently across all services:

```csharp
var app = builder.Build();

// Development-specific middleware
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();           // OpenAPI/Swagger endpoint
    app.UseSwaggerUi();         // Swagger UI
    app.ApplyMigrations();      // Auto-apply EF migrations
    await app.ApplySeedDataAsync(); // Seed test data
}

// Health checks endpoint
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// CORS configuration
app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

// Error handling
app.UseStatusCodePages();      // Handle HTTP status codes
app.UseExceptionHandler("/error"); // Global exception handling

// Authentication & Authorization
app.UseAuthentication();       // JWT token validation
app.UseAuthorization();        // Role/policy-based authorization

// Route endpoints
app.MapControllers();          // Map controller endpoints
app.MapRazorPages();          // Map Razor pages (if used)
```

#### **Middleware Explanations**

##### `app.UseOpenApi()` & `app.UseSwaggerUi()`

- **Purpose**: API documentation and testing interface
- **What it does**:
  - Exposes OpenAPI specification at `/swagger/v1/swagger.json`
  - Provides interactive Swagger UI for API testing
  - Only enabled in development environment

##### `app.UseHealthChecks("/health")`

- **Purpose**: Application health monitoring
- **What it does**:
  - Exposes health status at `/health` endpoint
  - Checks database connectivity, external services
  - Returns detailed health information in JSON format
- **Example Response**:

```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "IdentityDb": {
      "status": "Healthy",
      "duration": "00:00:00.0045678"
    },
    "KeycloakDb": {
      "status": "Healthy",
      "duration": "00:00:00.0034567"
    }
  }
}
```

##### `app.UseCors()`

- **Purpose**: Cross-Origin Resource Sharing configuration
- **What it does**:
  - Allows frontend applications to call APIs
  - Configures allowed origins, headers, and methods
  - **Note**: `AllowAnyOrigin()` should be restricted in production

##### `app.UseExceptionHandler("/error")`

- **Purpose**: Global exception handling
- **What it does**:
  - Catches unhandled exceptions
  - Converts to Problem Details format
  - Logs exceptions with context
  - Returns user-friendly error responses

##### `app.UseAuthentication()` & `app.UseAuthorization()`

- **Purpose**: Security middleware
- **What it does**:
  - **Authentication**: Validates JWT tokens from Keycloak
  - **Authorization**: Enforces role/policy-based access control
  - **Order matters**: Authentication must come before Authorization

### 🎮 **Controller Patterns**

All controllers follow consistent patterns for API endpoints:

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender) { }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">User registration details</param>
    /// <returns>Created user ID</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = request.Adapt<RegisterUserCommand>();
        var result = await Sender.Send(command);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetUser), new { id = result.Response }, result.Response)
            : HandleFailure(result);
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id:guid}")]
    [Authorize] // Requires authentication
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var query = new GetUserQuery(id);
        var result = await Sender.Send(query);

        return result.IsSuccess ? Ok(result.Response) : HandleFailure(result);
    }
}

// Base controller with common functionality
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult HandleFailure(Result result)
    {
        return result.Error.Code switch
        {
            "User.NotFound" => NotFound(CreateProblemDetails("User not found", StatusCodes.Status404NotFound)),
            "User.AlreadyExists" => Conflict(CreateProblemDetails("User already exists", StatusCodes.Status409Conflict)),
            "Validation.Error" => BadRequest(CreateProblemDetails("Validation failed", StatusCodes.Status400BadRequest)),
            _ => StatusCode(StatusCodes.Status500InternalServerError, CreateProblemDetails("Internal server error", StatusCodes.Status500InternalServerError))
        };
    }

    private ProblemDetails CreateProblemDetails(string title, int status)
    {
        return new ProblemDetails
        {
            Title = title,
            Status = status,
            Instance = HttpContext.Request.Path
        };
    }
}
```

#### **Controller Standards**

1. **API Versioning**: All controllers use `[ApiVersion("1.0")]`
2. **Route Convention**: `api/v{version:apiVersion}/[controller]`
3. **Response Types**: Explicit `[ProducesResponseType]` attributes
4. **Documentation**: XML comments for Swagger generation
5. **Error Handling**: Consistent error response format
6. **Authorization**: `[Authorize]` attributes where needed
7. **Validation**: Automatic through FluentValidation pipeline

#### **Request/Response Contracts**

```csharp
// Request DTO
public record RegisterUserRequest(
    [Required] string Email,
    [Required] string Password,
    [Required] string FirstName,
    [Required] string LastName,
    DateTime? BirthDate
);

// Response DTO
public record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    DateTime CreatedAt,
    bool IsEmailVerified
);

// Mapping Configuration (Mapster)
public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserRequest, RegisterUserCommand>();
        config.NewConfig<User, UserResponse>()
            .Map(dest => dest.IsEmailVerified, src => src.EmailConfirmed);
    }
}
```

## Development Guidelines

### 🎯 Best Practices

1. **Follow Clean Architecture**: Maintain clear layer boundaries
2. **Use CQRS**: Separate read and write operations
3. **Implement DDD**: Model rich domain objects
4. **Handle Errors Consistently**: Use Result pattern
5. **Validate Early**: Use FluentValidation
6. **Log Structurally**: Use Serilog with context
7. **Test Thoroughly**: Unit, integration, and contract tests
8. **Document APIs**: Maintain OpenAPI specifications

### 🚫 Anti-Patterns to Avoid

- ❌ Direct database access from controllers
- ❌ Business logic in infrastructure layer
- ❌ Tight coupling between services
- ❌ Inconsistent error handling
- ❌ Missing validation
- ❌ Poor logging practices

### 📋 Service Checklist

When creating a new service, ensure:

- [ ] Follows 4-layer Clean Architecture
- [ ] Implements CQRS with MediatR
- [ ] Uses shared BuildingBlocks
- [ ] Includes FluentValidation
- [ ] Implements health checks
- [ ] Integrates with Keycloak
- [ ] Uses structured logging
- [ ] Has proper error handling
- [ ] Includes API documentation
- [ ] Follows naming conventions

## Getting Started

To create a new service following this architecture:

1. **Copy the Identity service structure**
2. **Rename namespaces and projects**
3. **Adapt domain models to your business context**
4. **Implement service-specific business logic**
5. **Configure database and external integrations**
6. **Add service-specific validation rules**
7. **Create API contracts and mappings**
8. **Write comprehensive tests**

## Conclusion

The Identity service serves as the **architectural gold standard** for the YGZ microservices ecosystem. By following its patterns and structure, we ensure consistency, maintainability, and scalability across all services while promoting code reuse through shared building blocks.
