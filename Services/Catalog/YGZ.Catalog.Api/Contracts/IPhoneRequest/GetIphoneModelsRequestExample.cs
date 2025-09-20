using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using NJsonSchema;
using YGZ.Catalog.Api.Controllers;

namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

public class GetIphoneModelsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if (context.ControllerType == typeof(ProductController))
        {
            var operation = context.OperationDescription.Operation;

            // Set example for _page parameter  
            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_page");
            if (pageParam != null)
            {
                pageParam.Example = 1;
            }

            // Set example for _limit parameter  
            var limitParam = operation.Parameters.FirstOrDefault(p => p.Name == "_limit");
            if (limitParam != null)
            {
                limitParam.Example = 10;
            }

            // Set example for _code parameter  
            var colorParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productColors");
            if (colorParam != null)
            {
                colorParam.Example = new[] { "ultramarine", "pink" };

                List<string> colorList = new List<string>
               {
                   "ultramarine", "teal", "pink", "white", "black", "desert-titanium", "natural-titanium", "white-titanium", "black-titanium"
               };

                colorParam.Schema = new JsonSchema
                {
                    Type = JsonObjectType.String
                };
                foreach (var name in colorList)
                {
                    colorParam.Schema.Enumeration.Add(name);
                }
            }

            // Set example for _status parameter  
            var storageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productStorages");
            if (storageParam != null)
            {
                storageParam.Example = new[] { "128GB", "256GB" };

                List<string> storageList = new List<string> { "128GB", "256GB", "512GB", "1024GB" };

                storageParam.Schema = new JsonSchema
                {
                    Type = JsonObjectType.String
                };

                foreach (var name in storageList)
                {
                    storageParam.Schema.Enumeration.Add(name);
                }
            }

            var modelParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productModels");
            if (modelParam != null)
            {
                modelParam.Example = new[] { "iphone-16", "iphone-16e" };

                List<string> modelList = new List<string> { "iphone-15", "iphone-16", "iphone-16e", "iphone-16-pro" };

                modelParam.Schema = new JsonSchema
                {
                    Type = JsonObjectType.String
                };
                foreach (var name in modelList)
                {
                    modelParam.Schema.Enumeration.Add(name);
                }
            }

            var priceFromParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceFrom");
            if (priceFromParam != null)
            {
                priceFromParam.Example = "100";
            }

            var priceToParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceTo");
            if (priceToParam != null)
            {
                priceToParam.Example = "1000";
            }

            var sortParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceSort");
            if (sortParam != null)
            {
                sortParam.Example = "asc";

                List<string> sortList = new List<string> { "asc", "desc" };

                sortParam.Schema = new JsonSchema
                {
                    Type = JsonObjectType.String
                };

                foreach (var name in sortList)
                {
                    sortParam.Schema.Enumeration.Add(name);
                }
            }
        }
        return true; // Continue processing other processors  
    }
}
