using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
// using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace WebApi1.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        var jwtKey = "SuperSecretKey12345678901234567890";
        var key = Encoding.ASCII.GetBytes(jwtKey);

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization();

        return services;
    }

    public static void AddSwaggerJwtAuth(this SwaggerGenOptions options)
    {
        // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        // {
        //     Description = "Введите токен в формате: Bearer {token}",
        //     Name = "Authorization",
        //     In = ParameterLocation.Header,
        //     Type = SecuritySchemeType.ApiKey,
        //     Scheme = "Bearer"
        // });

//         options.AddSecurityRequirement(new OpenApiSecurityRequirement
//         {
//             {
//                 new OpenApiSecurityScheme
//                 {
//                     Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
//                 },
//                 new string[] {}
//             }
//         });
//
//         options.AddSecurityRequirement(new OpenApiSecurityRequirement
// {
//     {
//         new OpenApiSecurityScheme
//         {
//             Reference = new OpenApiReference
//             {
//                 Type = ReferenceType.SecurityScheme,
//                 Id = "Bearer"
//             }
//         },
//         Array.Empty<string>()
//     }
// });
    }
}