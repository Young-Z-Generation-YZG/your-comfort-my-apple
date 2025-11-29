using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Reflection;

namespace YGZ.Identity.Api.Extensions;

/// <summary>
/// Attribute to specify custom headers for API operations
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class SwaggerHeaderAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }
    public string DefaultValue { get; }
    public bool IsRequired { get; }

    public SwaggerHeaderAttribute(string name, string description = "", string defaultValue = "", bool isRequired = false)
    {
        Name = name;
        Description = description;
        DefaultValue = defaultValue;
        IsRequired = isRequired;
    }
}

/// <summary>
/// Operation processor that processes SwaggerHeader attributes
/// </summary>
public class SwaggerHeaderOperationProcessor : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check method-level attributes
        var methodAttributes = context.MethodInfo.GetCustomAttributes<SwaggerHeaderAttribute>();
        foreach (var attribute in methodAttributes)
        {
            AddHeaderParameter(context, attribute);
        }

        // Check controller-level attributes
        var controllerAttributes = context.ControllerType.GetCustomAttributes<SwaggerHeaderAttribute>();
        foreach (var attribute in controllerAttributes)
        {
            AddHeaderParameter(context, attribute);
        }

        return true;
    }

    private static void AddHeaderParameter(OperationProcessorContext context, SwaggerHeaderAttribute attribute)
    {
        // Check if parameter already exists
        var existingParam = context.OperationDescription.Operation.Parameters
            .FirstOrDefault(p => p.Name.Equals(attribute.Name, StringComparison.OrdinalIgnoreCase));

        if (existingParam == null)
        {
            context.OperationDescription.Operation.Parameters.Add(new NSwag.OpenApiParameter
            {
                Name = attribute.Name,
                Kind = NSwag.OpenApiParameterKind.Header,
                Type = NJsonSchema.JsonObjectType.String,
                IsRequired = attribute.IsRequired,
                Description = attribute.Description,
                Example = attribute.DefaultValue
            });
        }
    }
}
