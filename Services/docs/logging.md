# Logging in Services Documentation

## Overview

This document outlines the standardized logging approach used across all microservices in the YGZ system. The logging infrastructure is built on **Serilog** with **OpenTelemetry** integration, providing structured logging, centralized log aggregation, and observability capabilities.

## 🔧 **Configuration Setup**

### **1. NuGet Packages Required**

```xml
<PackageReference Include="Serilog.AspNetCore" Version="8.0.3" />
<PackageReference Include="Serilog.Sinks.Console" Version="7.0.0" />
<PackageReference Include="Serilog.Sinks.Seq" Version="8.0.0" />
<PackageReference Include="Serilog.Sinks.File" Version="7.0.0" />
<PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.9.0" />
<PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.9.0" />
```

### **2. appsettings.json Configuration**

#### **Development Environment**

```json
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.Seq",
      "Serilog.Sinks.File"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": { "serverUrl": "http://localhost:5341/" }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "rollingFileSizeLimit": true,
          "formatter": "Serilog.Formatting.Json.JsonFormatter"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
  }
}
```

#### **Production Environment**

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Seq"],
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Seq",
        "Args": { "serverUrl": "https://seq.yourdomain.com/" }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
  }
}
```

---

## 🚀 **Service Registration**

### **1. DependencyInjection.cs Setup**

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddMonitoringAndLogging(
        IServiceCollection services,
        WebApplicationBuilder builder)
    {
        // Add Serilog to Host
        builder.Host.AddSerilogExtension(builder.Configuration);

        // Add Keycloak OpenTelemetry
        services.AddKeycloakOpenTelemetryExtension();

        // Add OpenTelemetry Logging
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        return services;
    }
}
```

---

## 📝 **Usage Patterns**

### **1. Controller Logging**

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UsersController : ApiController
{
    private readonly ILogger<UsersController> _logger;
    private readonly ISender _sender;

    public UsersController(ILogger<UsersController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting user registration for email: {Email}",
            request.Email);

        try
        {
            var command = _mapper.Map<RegisterUserCommand>(request);
            var result = await _sender.Send(command, cancellationToken);

            if (result.IsSuccess)
            {
                _logger.LogInformation(
                    "User registered successfully. UserId: {UserId}",
                    result.Value);
                return Ok(result.Value);
            }

            _logger.LogWarning(
                "User registration failed for email: {Email}. Errors: {@Errors}",
                request.Email, result.Errors);
            return HandleFailure(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error during user registration for email: {Email}",
                request.Email);
            throw;
        }
    }
}
```

### **2. Service Layer Logging**

```csharp
public class IdentityService : IIdentityService
{
    private readonly ILogger<IdentityService> _logger;
    private readonly UserManager<User> _userManager;

    public IdentityService(
        ILogger<IdentityService> logger,
        UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result<User>> FindUserAsync(string email)
    {
        _logger.LogDebug("Searching for user with email: {Email}", email);

        try
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is null)
            {
                _logger.LogInformation("User not found with email: {Email}", email);
                return Errors.User.DoesNotExist;
            }

            _logger.LogDebug("User found: {UserId} for email: {Email}",
                existingUser.Id, email);
            return existingUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error finding user with email: {Email}. Method: {MethodName}",
                email, nameof(FindUserAsync));
            throw;
        }
    }
}
```

### **3. Repository Layer Logging**

```csharp
public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : ValueObject
{
    private readonly ILogger<GenericRepository<TEntity, TId>> _logger;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(
        DbContext dbContext,
        ILogger<GenericRepository<TEntity, TId>> logger)
    {
        _dbSet = dbContext.Set<TEntity>();
        _logger = logger;
    }

    public async Task<Result<TEntity>> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Retrieving {EntityType} with ID: {EntityId}",
            typeof(TEntity).Name, id);

        try
        {
            var result = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (result is null)
            {
                _logger.LogWarning(
                    "{EntityType} not found with ID: {EntityId}",
                    typeof(TEntity).Name, id);
                return Errors.Entity.NotFound;
            }

            _logger.LogDebug(
                "{EntityType} retrieved successfully. ID: {EntityId}",
                typeof(TEntity).Name, id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Database error retrieving {EntityType} with ID: {EntityId}. " +
                "Class: {ClassName}, Method: {MethodName}",
                typeof(TEntity).Name, id,
                nameof(GenericRepository<TEntity, TId>),
                nameof(GetByIdAsync));
            throw;
        }
    }
}
```

## 📊 **Log Output Examples**

### File

```File
{"Timestamp":"2025-08-29T01:17:27.5973729+07:00","Level":"Information","MessageTemplate":"Request starting {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {ContentType} {ContentLength}","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"Protocol":"HTTP/2","Method":"GET","ContentType":null,"ContentLength":null,"Scheme":"https","Host":"localhost:5055","PathBase":"","Path":"/api/v1/test/test-logging","QueryString":"","EventId":{"Id":1},"SourceContext":"Microsoft.AspNetCore.Hosting.Diagnostics","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6012349+07:00","Level":"Information","MessageTemplate":"Executing endpoint '{EndpointName}'","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"EndpointName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","EventId":{"Name":"ExecutingEndpoint"},"SourceContext":"Microsoft.AspNetCore.Routing.EndpointMiddleware","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6022443+07:00","Level":"Information","MessageTemplate":"Route matched with {RouteData}. Executing controller action with signature {MethodInfo} on controller {Controller} ({AssemblyName}).","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"RouteData":"{action = \"TestLogging\", controller = \"Test\"}","MethodInfo":"System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] TestLogging(System.Threading.CancellationToken)","Controller":"YGZ.Identity.Api.Controllers.TestController","AssemblyName":"YGZ.Identity.Api","EventId":{"Id":102,"Name":"ControllerActionExecuting"},"SourceContext":"Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker","ActionId":"8047f1f7-59b3-49c3-a31c-3386319a8bff","ActionName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6045538+07:00","Level":"Information","MessageTemplate":"Executing action method {ActionName} - Validation state: {ValidationState}","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"ActionName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","ValidationState":"Valid","EventId":{"Id":101,"Name":"ActionMethodExecuting"},"SourceContext":"Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker","ActionId":"8047f1f7-59b3-49c3-a31c-3386319a8bff","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6058272+07:00","Level":"Information","MessageTemplate":"Starting test logging {@RequestName},{@DateTimeUtc}","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"RequestName":"TestController","DateTimeUtc":"2025-08-28T18:17:27.6057211Z","SourceContext":"YGZ.Identity.Api.Controllers.TestController","ActionId":"8047f1f7-59b3-49c3-a31c-3386319a8bff","ActionName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6067841+07:00","Level":"Information","MessageTemplate":"Executed action method {ActionName}, returned result {ActionResult} in {ElapsedMilliseconds}ms.","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"ActionName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","ActionResult":"Microsoft.AspNetCore.Mvc.OkResult","ElapsedMilliseconds":0.9175,"EventId":{"Id":103,"Name":"ActionMethodExecuted"},"SourceContext":"Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker","ActionId":"8047f1f7-59b3-49c3-a31c-3386319a8bff","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6086560+07:00","Level":"Information","MessageTemplate":"Executing StatusCodeResult, setting HTTP status code {StatusCode}","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"StatusCode":200,"EventId":{"Id":1,"Name":"HttpStatusCodeResultExecuting"},"SourceContext":"Microsoft.AspNetCore.Mvc.StatusCodeResult","ActionId":"8047f1f7-59b3-49c3-a31c-3386319a8bff","ActionName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6095403+07:00","Level":"Information","MessageTemplate":"Executed action {ActionName} in {ElapsedMilliseconds}ms","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"ActionName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","ElapsedMilliseconds":5.3926,"EventId":{"Id":105,"Name":"ActionExecuted"},"SourceContext":"Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6105930+07:00","Level":"Information","MessageTemplate":"Executed endpoint '{EndpointName}'","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"EndpointName":"YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)","EventId":{"Id":1,"Name":"ExecutedEndpoint"},"SourceContext":"Microsoft.AspNetCore.Routing.EndpointMiddleware","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
{"Timestamp":"2025-08-29T01:17:27.6115036+07:00","Level":"Information","MessageTemplate":"Request finished {Protocol} {Method} {Scheme}://{Host}{PathBase}{Path}{QueryString} - {StatusCode} {ContentLength} {ContentType} {ElapsedMilliseconds}ms","TraceId":"d35dbb6dfb47c0bdf84dd2437a8bda20","SpanId":"29a231e52630ba95","Properties":{"ElapsedMilliseconds":14.51,"StatusCode":200,"ContentType":null,"ContentLength":0,"Protocol":"HTTP/2","Method":"GET","Scheme":"https","Host":"localhost:5055","PathBase":"","Path":"/api/v1/test/test-logging","QueryString":"","EventId":{"Id":2},"SourceContext":"Microsoft.AspNetCore.Hosting.Diagnostics","RequestId":"0HNF6BP7MIK0K:00000013","RequestPath":"/api/v1/test/test-logging","ConnectionId":"0HNF6BP7MIK0K"}}
```

### Console

```Console
[01:18:46 INF] Request starting HTTP/2 GET https://localhost:5055/api/v1/test/test-logging - null null
[01:18:46 INF] Executing endpoint 'YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)'
[01:18:46 INF] Route matched with {action = "TestLogging", controller = "Test"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] TestLogging(System.Threading.CancellationToken) on controller YGZ.Identity.Api.Controllers.TestController (YGZ.Identity.Api).
[01:18:46 INF] Executing action method YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api) - Validation state: Valid
[01:18:46 INF] Starting test logging TestController,08/28/2025 18:18:46
[01:18:46 INF] Executed action method YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api), returned result Microsoft.AspNetCore.Mvc.OkResult in 0.8423ms.
[01:18:46 INF] Executing StatusCodeResult, setting HTTP status code 200
[01:18:46 INF] Executed action YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api) in 4.1766ms
[01:18:46 INF] Executed endpoint 'YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)'
[01:18:46 INF] Request finished HTTP/2 GET https://localhost:5055/api/v1/test/test-logging - 200 0 null 14.9622ms
```

### Seq

```Seq
29 Aug 2025 01:20:06.115
Request finished HTTP/2 GET https://localhost:5055/api/v1/test/test-logging - 200 0 null 13.6543ms
29 Aug 2025 01:20:06.114
Executed endpoint 'YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)'
29 Aug 2025 01:20:06.114
Executed action YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api) in 3.7449ms
29 Aug 2025 01:20:06.113
Executing StatusCodeResult, setting HTTP status code 200
29 Aug 2025 01:20:06.112
Executed action method YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api), returned result Microsoft.AspNetCore.Mvc.OkResult in 0.8672ms.
29 Aug 2025 01:20:06.111
Starting test logging TestController,2025-08-28T18:20:06.1117015Z
29 Aug 2025 01:20:06.110
Executing action method YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api) - Validation state: Valid
29 Aug 2025 01:20:06.109
Route matched with {action = "TestLogging", controller = "Test"}. Executing controller action with signature System.Threading.Tasks.Task`1[Microsoft.AspNetCore.Mvc.IActionResult] TestLogging(System.Threading.CancellationToken) on controller YGZ.Identity.Api.Controllers.TestController (YGZ.Identity.Api).
29 Aug 2025 01:20:06.107
Executing endpoint 'YGZ.Identity.Api.Controllers.TestController.TestLogging (YGZ.Identity.Api)'
29 Aug 2025 01:20:06.102
Request starting HTTP/2 GET https://localhost:5055/api/v1/test/test-logging - null null
```

<!-- ---

## 🎯 **Logging Best Practices**

### **1. Log Levels Usage**

| Level           | Usage                                     | Example                                       |
| --------------- | ----------------------------------------- | --------------------------------------------- |
| **Trace**       | Detailed diagnostic information           | SQL queries, method entry/exit                |
| **Debug**       | Diagnostic information for debugging      | Variable values, state information            |
| **Information** | General application flow                  | Business operations, user actions             |
| **Warning**     | Unexpected but handled situations         | Retry attempts, fallback behavior             |
| **Error**       | Errors that don't stop the application    | Validation failures, business rule violations |
| **Fatal**       | Critical errors that stop the application | Unhandled exceptions, system failures         |

### **2. Structured Logging Patterns**

#### **Use Structured Properties**

```csharp
// ✅ Good: Structured logging with properties
_logger.LogInformation(
    "User {Action} completed. UserId: {UserId}, Email: {Email}, Duration: {Duration}ms",
    "registration", userId, email, duration);

// ❌ Bad: String concatenation
_logger.LogInformation("User registration completed. UserId: " + userId + ", Email: " + email);
```

#### **Include Context Information**

```csharp
// ✅ Good: Rich context information
_logger.LogInformation(
    "Order {OrderId} created for user {UserId} with {ItemCount} items. " +
    "Total: {TotalAmount:C}, PaymentMethod: {PaymentMethod}",
    orderId, userId, itemCount, totalAmount, paymentMethod);

// ❌ Bad: Minimal context
_logger.LogInformation("Order created");
```

### **3. Exception Logging**

```csharp
try
{
    // Business logic
}
catch (ValidationException ex)
{
    _logger.LogWarning(
        ex,
        "Validation failed for user {UserId}. Validation errors: {@ValidationErrors}",
        userId, ex.Errors);
    throw;
}
catch (DatabaseException ex)
{
    _logger.LogError(
        ex,
        "Database error occurred while processing user {UserId}. " +
        "Operation: {Operation}, Connection: {ConnectionString}",
        userId, operation, connectionString);
    throw;
}
catch (Exception ex)
{
    _logger.LogError(
        ex,
        "Unexpected error occurred while processing user {UserId}. " +
        "Operation: {Operation}, Context: {@Context}",
        userId, operation, new { UserId = userId, Operation = operation });
    throw;
}
```

---

## 🔍 **Log Enrichment & Context**

### **1. Log Context Enrichment**

```csharp
// Add correlation ID to all logs in a request
using (_logger.BeginScope(new Dictionary<string, object>
{
    ["CorrelationId"] = correlationId,
    ["UserId"] = userId,
    ["RequestPath"] = HttpContext.Request.Path
}))
{
    _logger.LogInformation("Processing request");
    // All logs in this scope will include the correlation ID
}
```

### **2. Custom Enrichers**

```csharp
// In Program.cs or Startup.cs
Log.Logger = new LoggerConfiguration()
    .Enrich.With<CustomEnricher>()
    .WriteTo.Console()
    .CreateLogger();

// Custom enricher implementation
public class CustomEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "ApplicationName", "YGZ.Identity.Api"));
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
            "Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"));
    }
}
```

---

## 📊 **Monitoring & Observability**

### **1. OpenTelemetry Integration**

```csharp
// Program.cs
builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;

    // Configure OTLP exporter
    logging.AddOtlpExporter(opts =>
    {
        opts.Endpoint = new Uri("http://localhost:4317");
    });
});

// appsettings.json
{
  "OTEL_EXPORTER_OTLP_ENDPOINT": "http://localhost:4317",
  "OTEL_SERVICE_NAME": "YGZ.Identity.Api"
}
```

### **2. Health Check Logging**

```csharp
services.AddHealthChecks()
    .AddCheck("Database", () =>
    {
        try
        {
            // Database health check logic
            _logger.LogDebug("Database health check completed successfully");
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database health check failed");
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    });
```

---

## 🚨 **Error Handling & Logging**

### **1. Global Exception Handler**

```csharp
// GlobalExceptionHandler.cs
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Unhandled exception occurred. Path: {Path}, Method: {Method}, " +
            "User: {UserId}, CorrelationId: {CorrelationId}",
            httpContext.Request.Path,
            httpContext.Request.Method,
            httpContext.User?.FindFirst("sub")?.Value,
            httpContext.TraceIdentifier);

        // Return standardized error response
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred while processing your request",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "Please try again later or contact support if the problem persists"
        };

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
```

### **2. Request Logging Middleware**

```csharp
// Uncomment in Program.cs for request logging
// app.UseSerilogRequestLogging(options =>
// {
//     options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
//     {
//         diagnosticContext.Set("UserId",
//             httpContext.User?.FindFirst("sub")?.Value ?? "anonymous");
//         diagnosticContext.Set("CorrelationId",
//             httpContext.TraceIdentifier);
//     };
// });
```

---

## 🔧 **Configuration Options**

### **1. Serilog Configuration Options**

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning",
        "System.Net.Http": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      },
      {
        "Name": "Seq",
        "Args": {
          "serverUrl": "http://localhost:5341/",
          "apiKey": "your-api-key"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 31,
          "fileSizeLimitBytes": 10485760,
          "formatter": "Serilog.Formatting.Json.JsonFormatter"
        }
      }
    ],
    "Enrich": [
      "FromLogContext",
      "WithMachineName",
      "WithThreadId",
      "WithEnvironmentName"
    ]
  }
}
```

### **2. Environment-Specific Configuration**

```csharp
// Program.cs
var environment = builder.Environment.EnvironmentName;

if (environment == "Development")
{
    // Development logging - verbose, multiple sinks
    builder.Host.AddSerilogExtension(builder.Configuration);
}
else
{
    // Production logging - structured, centralized
    builder.Host.AddSerilogExtension(builder.Configuration);

    // Add production-specific logging
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.WithProperty("Environment", environment)
        .Enrich.WithProperty("Application", "YGZ.Identity.Api")
        .CreateLogger();
}
```

---

## 📈 **Performance Considerations**

### **1. Logging Performance Tips**

```csharp
// ✅ Good: Use structured logging with minimal string operations
_logger.LogInformation("User {UserId} logged in from {IPAddress}", userId, ipAddress);

// ✅ Good: Use conditional logging for expensive operations
if (_logger.IsEnabled(LogLevel.Debug))
{
    var expensiveData = GetExpensiveData();
    _logger.LogDebug("Expensive data: {@Data}", expensiveData);
}

// ❌ Bad: Expensive string operations in logging
_logger.LogInformation("User " + userId + " logged in from " + ipAddress + " at " + DateTime.Now);
```

### **2. Async Logging**

```csharp
// For high-performance scenarios, consider async logging
public async Task LogAsync(string message, object[] args)
{
    await Task.Run(() => _logger.LogInformation(message, args));
}
```

---

## 🔍 **Troubleshooting**

### **1. Common Issues**

#### **Logs Not Appearing**

- Check log level configuration
- Verify sink configuration (Console, Seq, File)
- Check file permissions for file sinks
- Verify network connectivity for remote sinks

#### **Performance Issues**

- Reduce log verbosity in production
- Use conditional logging for expensive operations
- Consider async logging for high-throughput scenarios
- Monitor log file sizes and rotation

### **2. Debug Configuration**

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Serilog": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

---

## 📚 **Additional Resources**

- [Serilog Documentation](https://serilog.net/)
- [OpenTelemetry .NET](https://opentelemetry.io/docs/instrumentation/net/)
- [Seq Log Server](https://datalust.co/docs/)
- [Structured Logging Best Practices](https://serilog.net/structured-logging-concepts) -->
