using CleanCodeScaffold.Application.Dtos;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CleanCodeScaffold.Api.Util
{
    public class FiltersSwaggerConfigs : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            foreach (var parameter in context.ApiDescription.ActionDescriptor.Parameters)
            {
                if (parameter.ParameterType == typeof(IEnumerable<FilterVM>))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "filters[0].key",
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema { Type = "string" },
                        Required = false,
                        Description = "Property name for the first filter"
                    });

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "filters[0].value",
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema { Type = "string" },
                        Required = false,
                        Description = "Value for the first filter"
                    });

                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "filters[0].operator",
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema { Type = "string" },
                        Required = false,
                        Description = "Operator for the first filter"
                    });
                }
            }
        }
    }
}
