using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace YGZ.Catalog.Api.Contracts.ReviewRequest;

public class GetReviewsQueryParamsRequestExample : IOperationProcessor
{
    public bool Process(OperationProcessorContext context)
    {
        if (context.ControllerType == typeof(GetReviewsQueryParamsRequest))
        {
            var operation = context.OperationDescription.Operation;

            var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "_page");
            if (pageParam != null)
            {
                pageParam.Example = 1;
            }

            var limitParam = operation.Parameters.FirstOrDefault(p => p.Name == "_limit");
            if (limitParam != null)
            {
                limitParam.Example = 10;
            }

            var sortByParam = operation.Parameters.FirstOrDefault(p => p.Name == "_sortBy");
            if (sortByParam != null)
            {
                sortByParam.Example = "rating";

                List<string> modelList = ["rating"];

                sortByParam.Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                };
                foreach (var name in modelList)
                {
                    sortByParam.Schema.Enumeration.Add(name);
                }
            }

            var sortParam = operation.Parameters.FirstOrDefault(p => p.Name == "_sortOrder");
            if (sortParam != null)
            {
                sortParam.Example = "desc";

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

        return true;
    }
}
