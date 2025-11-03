using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace YGZ.Identity.Api.Extensions;

/// <summary>
/// Operation processor that adds custom headers to all API operations
/// </summary>
public class CustomHeaderOperationProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Add X-Tenant-Id header
        context.OperationDescription.Operation.Parameters.Add(new NSwag.OpenApiParameter
        {
            Name = "X-TenantId",
            Kind = NSwag.OpenApiParameterKind.Header,
            Type = NJsonSchema.JsonObjectType.String,
            IsRequired = false,
            Description = "Tenant identifier for multi-tenant operations",
            Example = "tenant-123"
        });

        // Add X-Request-Id header
        context.OperationDescription.Operation.Parameters.Add(new NSwag.OpenApiParameter
        {
            Name = "X-Request-Id",
            Kind = NSwag.OpenApiParameterKind.Header,
            Type = NJsonSchema.JsonObjectType.String,
            IsRequired = false,
            Description = "Unique request identifier for tracing",
            Example = Guid.NewGuid().ToString()
        });

        // Add X-Correlation-Id header
        context.OperationDescription.Operation.Parameters.Add(new NSwag.OpenApiParameter
        {
            Name = "X-Correlation-Id",
            Kind = NSwag.OpenApiParameterKind.Header,
            Type = NJsonSchema.JsonObjectType.String,
            IsRequired = false,
            Description = "Correlation identifier for request tracking",
            Example = Guid.NewGuid().ToString()
        });

        return true;
    }
}
