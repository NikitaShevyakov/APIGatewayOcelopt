using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi2.Models;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Достаем все метаданные эндпоинта (включая атрибуты контроллера и глобальные)
        var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

        // Проверяем наличие любого атрибута авторизации (или интерфейса IAuthorizeData)
        var hasAuthorize = metadata.Any(m => m is IAuthorizeData);
        var hasAllowAnonymous = metadata.Any(m => m is IAllowAnonymous);

        if (hasAuthorize && !hasAllowAnonymous)
        {
            var schemeReference = new OpenApiSecuritySchemeReference("Bearer", null);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                [schemeReference] = []
            };
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(securityRequirement);

            // Добавляем стандартные ответы для наглядности в UI
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
        }
    }
}
