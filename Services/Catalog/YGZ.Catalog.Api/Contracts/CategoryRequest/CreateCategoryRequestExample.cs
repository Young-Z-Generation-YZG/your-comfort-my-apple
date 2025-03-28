﻿using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.CategoryRequest;

public class CreateCategoryRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreateCategoryRequest))
        {
            context.Schema.Example = new
            {
                name = "iPhone",
                description = "iPhone categories.",
                parent_id = "67346f7549189f7314e4ef0c"
            };
        }
    }
}