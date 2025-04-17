using NSwag.Generation.Processors.Contexts;
using NSwag.Generation.Processors;
using NJsonSchema;
using YGZ.Catalog.Api.Controllers;

namespace YGZ.Catalog.Api.Contracts;

public class GetProductsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        // Check if this is the GetOrders method in OrderingController
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
            var colorParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productColor");
            if (colorParam != null)
            {
                colorParam.Example = "ultramarine";

                List<string> colorList = ["ultramarine", "teal", "pink", "white", "black", "desert-titanium", "natural-titanium", "white-titanium", "black-titanium"];

                colorParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                };
                foreach (var name in colorList)
                {
                    colorParam.Schema.Enumeration.Add(name);
                }
            }

            // Set example for _status parameter
            var storageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_productStorage");
            if (storageParam != null)
            {
                storageParam.Example = "256";

                List<string> storageList = ["128", "256", "512", "1024"];

                storageParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                };

                foreach (var name in storageList)
                {
                    storageParam.Schema.Enumeration.Add(name);
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

                List<string> sortList = ["asc", "desc"];

                sortParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
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
