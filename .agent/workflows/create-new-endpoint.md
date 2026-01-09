---
description: How to create a new API endpoint (GET/POST/...) using CQRS and MediatR
---

This workflow guides you through creating a new endpoint in the existing microservices architecture. The architecture follows Clean Architecture with CQRS (Command Query Responsibility Segregation) pattern using MediatR.

## 1. Identify the Location
Determine which Service (e.g., `Catalog`, `Basket`) and which Entity (e.g., `Categories`, `Products`) the endpoint belongs to.

## 2. Define the Contract (API Layer)
Create the Request and Response DTOs in the API project.
- **Location**: `Services\{Service}\YGZ.{Service}.Api\Contracts\{Entity}Request`
- **Naming**: `{Action}{Entity}Request.cs` (e.g., `CreateCategoryRequest.cs`, `GetProductRequest.cs`)

## 3. Implement Application Logic (Application Layer)
Create the Command (for POST/PUT/DELETE) or Query (for GET) in the Application project.

### Step 3a: Create Directory Structure
- **Location**: `Services\{Service}\YGZ.{Service}.Application\{Entity}\{Commands|Queries}\{Action}{Entity}`
- **Example**: `d:\projects\TLCN_ADMIN\Services\Catalog\YGZ.Catalog.Application\Categories\Commands\CreateCategory`

### Step 3b: Create the Command/Query
Create a class implementing `IRequest<Result<TResponse>>`.
```csharp
// Example: CreateCategoryCommand.cs
public record CreateCategoryCommand(string Name, string Description) : IRequest<Result<Guid>>;
```

### Step 3c: Create the Handler
Create a class implementing `IRequestHandler<TRequest, Result<TResponse>>`.
```csharp
// Example: CreateCategoryHandler.cs
internal sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    // Inject other dependencies like Repositories or Services

    public CreateCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // 1. Validation (if not handled by pipeline)
        // 2. Domain Logic
        // 3. Persistence
        
        return Result.Success(newId);
        // OR
        // return Result.Failure<Guid>(Error.NotFound ...);
    }
}
```

### Step 3d: Create the Validator (Optional but Recommended)
Use FluentValidation.
```csharp
// Example: CreateCategoryValidator.cs
public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
```

## 4. Expose the Endpoint (API Layer)
Add the endpoint to the corresponding Controller.

- **Location**: `Services\{Service}\YGZ.{Service}.Api\Controllers\{Entity}Controller.cs`
- **Base Class**: Ensure controller inherits from `ApiController`.

```csharp
[HttpPost] // or [HttpGet]
public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken cancellationToken)
{
    // Map Request to Command
    // You might need to add mapping config in Mapster (Mappings folder) if automatic mapping doesn't work or is complex
    var command = _mapper.Map<CreateCategoryCommand>(request);

    // Send to MediatR
    var result = await _sender.Send(command, cancellationToken);

    // Return Result
    return result.Match(
        onSuccess: value => Ok(value),
        onFailure: error => HandleFailure(error)
    );
}
```

## 5. Add Swagger Example (Recommended)
This step ensures your API documentation has realistic examples for the request body.

### Step 5a: Create the Example Class
Create a class implementing `ISchemaProcessor` in the same directory as the Request DTO or in a nearby `Examples` folder (usually inside `Contracts` or under local folder).

- **Location**: `Services\{Service}\YGZ.{Service}.Api\Contracts\{Entity}Request` (adjacent to request file)
- **Code**:
```csharp
using NJsonSchema.Generation;

public class CreateCategoryRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateCategoryRequest))
        {
            context.Schema.Example = new
            {
                name = "Electronics",
                description = "Gadgets and devices",
            };
        }
    }
}
```

### Step 5b: Register in Swagger Extensions
Register the processor in `SwaggerExtensions.cs` or `SwaggerExtension.cs`.

- **Location**: `Services\{Service}\YGZ.{Service}.Api\Extensions\SwaggerExtension.cs` (or `SwaggerExtensions.cs`)
- **Action**: Add the processor to `settings.SchemaSettings.SchemaProcessors`.

```csharp
services.AddOpenApiDocument((settings, sp) =>
{
    // ... existing configuration ...
    
    // Add custom schema processor
    settings.SchemaSettings.SchemaProcessors.Add(new CreateCategoryRequestExample());
});
```

## 6. Register Mappings (If needed)
If using Mapster and the mapping isn't trivial, register it in `Services\{Service}\YGZ.{Service}.Api\Mappings`.

## 7. Build and Test
- Run the service.
- Verify the endpoint appears in Swagger/OpenAPI with the example.
- Test with valid and invalid data.