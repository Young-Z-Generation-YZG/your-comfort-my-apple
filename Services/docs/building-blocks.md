# Building Blocks Documentation

## Overview

The **BuildingBlocks** directory contains shared libraries and common patterns used across all microservices in the YGZ system. These building blocks ensure consistency, reduce code duplication, and provide standardized implementations of cross-cutting concerns.

## Architecture Components

```
BuildingBlocks/
├── YGZ.BuildingBlocks.Shared/      # Core shared patterns and utilities
└── YGZ.BuildingBlocks.Messaging/   # Inter-service communication
```

---

## 📦 YGZ.BuildingBlocks.Shared

### Purpose

Contains fundamental patterns, abstractions, and utilities that all services depend on for consistent behavior and implementation.

### Key Components

#### 🎯 **CQRS Abstractions**

Provides base interfaces for Command Query Responsibility Segregation pattern:

```csharp
// Command Interface
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }

// Command Handler Interface
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse> { }

// Query Interface
public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

// Query Handler Interface
public interface IQueryHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : IQuery<TResponse> { }
```

**Usage Example:**

```csharp
// Command Definition
public record CreateUserCommand(string Email, string Password) : ICommand<Result<Guid>>;

// Command Handler Implementation
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Implementation logic
        return Result<Guid>.Success(Guid.NewGuid());
    }
}
```

#### ✅ **Result Pattern**

Standardized error handling and response pattern across all services:

```csharp
// Base Result Class
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
}

// Generic Result Class
public class Result<TResponse> : Result
{
    public TResponse? Response { get; }

    public static Result<TResponse> Success(TResponse response) => new(true, response, Error.NoneError);
    public static Result<TResponse> Failure(Error error) => new(false, default!, error);

    // Implicit conversions
    public static implicit operator Result<TResponse>(TResponse response) => Success(response);
    public static implicit operator Result<TResponse>(Error error) => Failure(error);
}
```

**Benefits:**

- **Consistent Error Handling**: All operations return Result<T> for uniform error handling
- **Type Safety**: Compile-time guarantees for success/failure scenarios
- **Functional Programming**: Enables railway-oriented programming patterns

#### 🔍 **Validation Pipeline**

Automatic validation using FluentValidation integrated with MediatR pipeline:

```csharp
public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
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
                .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage, Assembly.GetExecutingAssembly().FullName!))
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

**Features:**

- **Automatic Validation**: Validates all commands/queries before execution
- **Consistent Error Format**: Standardized validation error responses
- **Pipeline Integration**: Seamlessly integrated with MediatR pipeline

#### 🏗️ **Domain Primitives**

Base classes for Domain-Driven Design implementation:

```csharp
// Base Entity with Domain Events
public abstract class Entity<TId> : IEquatable<Entity<TId>>, IAggregate
    where TId : ValueObject
{
    public TId Id { get; protected set; }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] events = _domainEvents.ToArray();
        _domainEvents.Clear();
        return events;
    }
}

// Value Object Base Class
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetAtomicValues();

    public override bool Equals(object obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}

// Domain Event Interface
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    DateTime OccurredOn => DateTime.Now;
    string EventType => GetType().AssemblyQualifiedName!;
}
```

#### 🔧 **Common Extensions**

##### **Keycloak Authentication Extension**

```csharp
public static class KeycloakIdentityServerExtension
{
    public static IServiceCollection AddKeycloakIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT Bearer authentication setup
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Keycloak configuration
                options.Authority = configuration["Keycloak:Authority"];
                options.Audience = configuration["Keycloak:Audience"];
                options.RequireHttpsMetadata = false;
            });

        // Authorization policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationConstants.Policies.RequireClientRole, policy =>
                policy.RequireAuthenticatedUser());
        });

        return services;
    }
}
```

##### **OpenTelemetry Extension**

```csharp
public static class OpenTelemetryExtension
{
    public static IServiceCollection AddKeycloakOpenTelemetryExtensions(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddEntityFrameworkCoreInstrumentation();
            })
            .WithMetrics(builder =>
            {
                builder.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
            });

        return services;
    }
}
```

##### **FluentValidation Extension**

```csharp
public static class FluentValidationExtension
{
    public static IServiceCollection AddFluentValidationExtension(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        return services;
    }
}
```

##### **Mapping Extension (Mapster)**

```csharp
public static class MappingExtension
{
    public static IServiceCollection AddSharedExtensions(this IServiceCollection services, Assembly assembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assembly);

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
```

#### 📄 **Shared Contracts**

Standardized DTOs and response models used across services:

##### **Authentication Contracts**

```csharp
[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record LoginResponse()
{
    public required string UserEmail { get; init; }
    public required string Username { get; init; }
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }
    public double? AccessTokenExpiresInSeconds { get; init; }
    public double? RefreshTokenExpiresInSeconds { get; init; }
    public required string VerificationType { get; init; }
    public Dictionary<string, string>? Params { get; init; }
}

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record TokenResponse
{
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public int RefreshExpiresIn { get; init; }
    public string RefreshToken { get; init; }
    public string TokenType { get; init; }
    public string Scope { get; init; }
}
```

##### **Pagination Contracts**

```csharp
[JsonConverter(typeof(SnakeCaseSerializer))]
public class PaginationResponse<TData>
{
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public IEnumerable<TData> Items { get; set; } = new List<TData>();
    public PaginationLinks Links { get; set; } = new PaginationLinks("", "", "", "");
}

public sealed record PaginationLinks(string? First, string? Prev, string? Next, string? Last) { }
```

##### **Common Response Models**

```csharp
// Product-related responses
[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record ColorResponse
{
    required public string ColorName { get; init; }
    required public string ColorHex { get; init; }
    required public string ColorImage { get; init; }
    public int? ColorOrder { get; init; }
}

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record ImageResponse
{
    required public string ImageId { get; init; }
    required public string ImageUrl { get; init; }
    required public string ImageName { get; init; }
    required public string ImageDescription { get; init; }
    public decimal ImageWidth { get; init; }
    public decimal ImageHeight { get; init; }
    public decimal ImageBytes { get; init; }
    public int? ImageOrder { get; init; }
}
```

#### 🛠️ **Utility Classes**

##### **Snake Case Serialization**

```csharp
public class SnakeCaseSerializer : JsonConverter<object>
{
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        var jsonSerializerOptions = new JsonSerializerOptions(options)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        JsonSerializer.Serialize(writer, value, value.GetType(), jsonSerializerOptions);
    }

    private static string ToSnakeCase(string str)
    {
        return Regex.Replace(str, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
```

##### **Expression Builder for Dynamic Queries**

```csharp
public static class ExpressionBuilder
{
    public static Expression<Func<T, bool>> New<T>()
    {
        return f => true;
    }

    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param)
        );
        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T));
        var body = Expression.OrElse(
            Expression.Invoke(left, param),
            Expression.Invoke(right, param)
        );
        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
```

##### **VnPay Payment Integration**

```csharp
public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new();
    private readonly SortedList<string, string> _responseData = new();

    public PaymentResponseModel GetFullResponseData(IQueryCollection collection, string hashSecret)
    {
        foreach (var (key, value) in collection)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                AddResponseData(key, value);
            }
        }

        var orderId = GetResponseData("vnp_TxnRef");
        var vnpayTranId = GetResponseData("vnp_TransactionNo");
        var vnpResponseCode = GetResponseData("vnp_ResponseCode");
        var vnpSecureHash = collection["vnp_SecureHash"];

        bool checkSignature = ValidateSignature(vnpSecureHash, hashSecret);

        return new PaymentResponseModel
        {
            Success = checkSignature && vnpResponseCode == "00",
            PaymentMethod = "VnPay",
            OrderDescription = GetResponseData("vnp_OrderInfo"),
            OrderId = orderId,
            PaymentId = vnpayTranId,
            TransactionId = vnpayTranId,
            Token = vnpSecureHash,
            VnPayResponseCode = vnpResponseCode
        };
    }
}
```

---

## 📨 YGZ.BuildingBlocks.Messaging

### Purpose

Handles inter-service communication through message brokers, enabling event-driven architecture and loose coupling between microservices.

### Key Components

#### 🔄 **Integration Events**

Base class for all integration events:

```csharp
public record IntegrationEvent
{
    public Guid Id => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;
}
```

#### 📦 **Service-Specific Integration Events**

##### **Basket Service Events**

```csharp
public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; } = default!;
    public string CustomerId { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string PaymentMethod { get; set; } = default!;
    public string ContactName { get; set; } = default!;
    public string ContactPhoneNumber { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Province { get; set; } = default!;
    public string Country { get; set; } = default!;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal SubTotalAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public List<OrderItemIntegrationEvent> OrderItems { get; set; } = new();
}

public record OrderItemIntegrationEvent()
{
    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductColorName { get; set; }
    public required decimal ProductUnitPrice { get; set; }
    public required string ProductNameTag { get; set; }
    public required string ProductImage { get; set; }
    public required string ProductSlug { get; set; }
    public required int Quantity { get; set; }
    public PromotionIntergrationEvent? Promotion { get; set; }
}
```

##### **Catalog Service Events**

```csharp
public record ReviewCreatedIntegrationEvent : IntegrationEvent
{
    public required string ReviewId { get; set; }
    public required string OrderItemId { get; set; }
    public required string CustomerId { get; set; }
    public required string ReviewContent { get; set; }
    public required int ReviewStar { get; set; }
    public required bool IsReviewed { get; set; }
}
```

##### **Ordering Service Events**

```csharp
public record OrderFullfilmentIntegrationEvent : IntegrationEvent { }
```

#### 🚌 **MassTransit Configuration**

RabbitMQ-based message broker setup:

```csharp
public static class Extensions
{
    public static IServiceCollection AddMessageBrokerExtensions(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetSection(MessageBrokerSettings.SettingKey));

        services.AddMassTransit(busConfigurator =>
        {
            if (assembly != null)
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.AddConsumers(assembly);
            }

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
```

#### ⚙️ **Message Broker Settings**

```csharp
public class MessageBrokerSettings
{
    public const string SettingKey = "MessageBrokerSettings";

    public string Host { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
```

---

## 📋 **Dependencies and Technology Stack**

### YGZ.BuildingBlocks.Shared Dependencies

```xml
<PackageReference Include="Ardalis.SmartEnum" Version="8.2.0" />
<PackageReference Include="AspNetCore.HealthChecks.NpgSql" Version="8.0.2" />
<PackageReference Include="AspNetCore.HealthChecks.Redis" Version="8.0.1" />
<PackageReference Include="AspNetCore.HealthChecks.UI.Client" Version="8.0.1" />
<PackageReference Include="CloudinaryDotNet" Version="1.27.5" />
<PackageReference Include="Keycloak.AuthServices.Authentication" Version="2.5.3" />
<PackageReference Include="Keycloak.AuthServices.Authorization" Version="2.5.5" />
<PackageReference Include="Keycloak.AuthServices.OpenTelemetry" Version="1.0.0" />
<PackageReference Include="Mapster" Version="7.4.0" />
<PackageReference Include="Mapster.DependencyInjection" Version="1.0.1" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.12" />
<PackageReference Include="Microsoft.Extensions.Caching.StackExchangeRedis" Version="8.0.14" />
<PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="9.2.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.11.0" />
<PackageReference Include="MediatR" Version="12.4.1" />
<PackageReference Include="NSwag.AspNetCore" Version="14.2.0" />
<PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.9.0" />
```

### YGZ.BuildingBlocks.Messaging Dependencies

```xml
<PackageReference Include="MassTransit" Version="8.3.2" />
<PackageReference Include="MassTransit.RabbitMQ" Version="8.3.2" />
<PackageReference Include="Microsoft.Extensions.Options" Version="8.0.2" />
```

---

## 🎯 **Usage Patterns**

### **1. Service Registration Pattern**

Every service follows this pattern in their `DependencyInjection.cs`:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Add MediatR with CQRS
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

        // Add FluentValidation with Pipeline Behavior
        services.AddFluentValidationExtension(assembly);

        // Add Mapster for object mapping
        services.AddSharedExtensions(assembly);

        return services;
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Authentication
        services.AddKeycloakIdentityServerExtension(configuration);

        // Add OpenTelemetry
        services.AddKeycloakOpenTelemetryExtensions();

        // Add Message Broker
        services.AddMessageBrokerExtensions(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
```

### **2. Command/Query Implementation Pattern**

```csharp
// Command
public record CreateProductCommand(string Name, decimal Price) : ICommand<Result<Guid>>;

// Command Handler
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Business logic
        return Result<Guid>.Success(Guid.NewGuid());
    }
}

// Validator
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

### **3. Integration Event Publishing Pattern**

```csharp
// Publishing an integration event
public class OrderService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public async Task CompleteOrderAsync(Order order)
    {
        // Complete order logic

        // Publish integration event
        await _publishEndpoint.Publish(new OrderFullfilmentIntegrationEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            // ... other properties
        });
    }
}

// Consuming an integration event
public class OrderFullfilmentConsumer : IConsumer<OrderFullfilmentIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderFullfilmentIntegrationEvent> context)
    {
        var orderEvent = context.Message;

        // Handle the integration event
        // Update inventory, send notifications, etc.
    }
}
```

---

## 🔑 **Key Benefits**

### **1. Consistency Across Services**

- **Standardized Patterns**: All services use the same CQRS, validation, and error handling patterns
- **Uniform APIs**: Consistent request/response formats across all endpoints
- **Shared Contracts**: Common DTOs prevent contract drift between services

### **2. Reduced Code Duplication**

- **Reusable Components**: Common functionality implemented once and shared
- **Extension Methods**: Standardized service registration and configuration
- **Base Classes**: Domain primitives and abstractions reduce boilerplate

### **3. Maintainability**

- **Centralized Updates**: Changes to common patterns propagate to all services
- **Type Safety**: Strong typing prevents runtime errors
- **Documentation**: Self-documenting code through well-defined interfaces

### **4. Scalability**

- **Event-Driven Architecture**: Loose coupling through integration events
- **Async Communication**: Non-blocking inter-service communication
- **Resilience Patterns**: Built-in retry, circuit breaker, and timeout handling

### **5. Developer Experience**

- **IntelliSense Support**: Strong typing provides excellent IDE support
- **Consistent APIs**: Developers can easily move between services
- **Clear Separation**: Well-defined boundaries between layers and concerns

---

## 🚀 **Getting Started**

### **1. Reference Building Blocks**

Add references to your service projects:

```xml
<ProjectReference Include="..\..\BuildingBlocks\YGZ.BuildingBlocks.Shared\YGZ.BuildingBlocks.Shared.csproj" />
<ProjectReference Include="..\..\BuildingBlocks\YGZ.BuildingBlocks.Messaging\YGZ.BuildingBlocks.Messaging.csproj" />
```

### **2. Configure Services**

In your `DependencyInjection.cs`:

```csharp
services.AddSharedExtensions(Assembly.GetExecutingAssembly());
services.AddFluentValidationExtension(Assembly.GetExecutingAssembly());
services.AddKeycloakIdentityServerExtension(configuration);
services.AddMessageBrokerExtensions(configuration, Assembly.GetExecutingAssembly());
```

### **3. Implement CQRS**

Create commands, queries, and handlers following the established patterns.

### **4. Use Result Pattern**

Return `Result<T>` from all service methods for consistent error handling.

### **5. Define Integration Events**

Create integration events for inter-service communication following the naming conventions.

---

## 📝 **Best Practices**

1. **Always use Result Pattern** for service methods
2. **Implement validation** for all commands and queries
3. **Follow naming conventions** for integration events
4. **Use snake_case** for JSON serialization
5. **Leverage domain events** for business logic side effects
6. **Keep integration events immutable** using record types
7. **Document all public contracts** with XML comments
8. **Use strong typing** wherever possible
9. **Follow SOLID principles** in all implementations
10. **Test shared components thoroughly** as they affect all services

The BuildingBlocks provide the foundation for a robust, maintainable, and scalable microservices architecture that promotes consistency and reduces development overhead across the entire YGZ system.
