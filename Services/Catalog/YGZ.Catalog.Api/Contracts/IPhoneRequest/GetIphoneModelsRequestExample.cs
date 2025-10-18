using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

public class GetIphoneModelsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if (context.ControllerType == typeof(GetIphoneModelsRequest))
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
            var colorParam = operation.Parameters.FirstOrDefault(p => p.Name == "_colors");
            if (colorParam != null)
            {
                colorParam.Example = new[] { "ultramarine", "pink" };

                List<string> colorList = new List<string>
               {
                   "ULTRAMARINE", "TEAL", "PINK", "WHITE", "BLACK", "DESERT-TITANIUM", "NATURAL-TITANIUM", "WHITE-TITANIUM", "BLACK-TITANIUM"
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
            var storageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_storages");
            if (storageParam != null)
            {
                storageParam.Example = new[] { "128GB", "256GB" };

                List<string> storageList = new List<string> { "128GB", "256GB", "512GB", "1TB" };

                storageParam.Schema = new JsonSchema
                {
                    Type = JsonObjectType.String
                };

                foreach (var name in storageList)
                {
                    storageParam.Schema.Enumeration.Add(name);
                }
            }

            var modelParam = operation.Parameters.FirstOrDefault(p => p.Name == "_models");
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

            var priceFromParam = operation.Parameters.FirstOrDefault(p => p.Name == "_minPrice");
            if (priceFromParam != null)
            {
                priceFromParam.Example = "100";
            }

            var priceToParam = operation.Parameters.FirstOrDefault(p => p.Name == "_maxPrice");
            if (priceToParam != null)
            {
                priceToParam.Example = "1000";
            }

            var sortParam = operation.Parameters.FirstOrDefault(p => p.Name == "_priceSort");
            if (sortParam != null)
            {
                sortParam.Example = "ASC";

                List<string> sortList = new List<string> { "ASC", "DESC" };

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
