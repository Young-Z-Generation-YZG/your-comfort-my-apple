using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.OpenApi;

public class SwaggerConfiguration : IConfigureOptions<SwaggerGenOptions>, IOperationFilter
{
    private readonly IApiVersionDescriptionProvider _provider;
    public SwaggerConfiguration(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.ParameterDescriptions.Any(p => p.Source == BindingSource.Form))
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = context.SchemaGenerator.GenerateSchema(typeof(CreateProductItemRequest), context.SchemaRepository)
                    }
                }
            };
        }
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "Catalog Service",
            Version = description.ApiVersion.ToString(),
            Description = "There are APIs used for authentication and authorization",
            Contact = new OpenApiContact
            {
                Name = "Lê Xuân Bách",
                Email = "lxbachit03.working@gmail.com",
                Url = new Uri("https://github.com/bachlex03")
            },
            License = new OpenApiLicense
            {
                Name = "Use under LICX"
            }
        };

        

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
