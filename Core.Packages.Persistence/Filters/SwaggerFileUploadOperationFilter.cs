using Microsoft.AspNetCore.Http;

namespace Core.Packages.Persistence.Filters
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Linq;

    public class SwaggerFileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Eğer operasyon parametreleri boşsa, işlem yapma
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            // Tüm parametreleri al
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IEnumerable<IFormFile>))
                .ToList();

            // Eğer IFormFile varsa multipart/form-data olarak ekle
            if (fileParams.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = fileParams.ToDictionary(
                                    p => p.Name,
                                    p => new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    })
                            }
                        }
                    }
                };
            }
        }
    }

}
