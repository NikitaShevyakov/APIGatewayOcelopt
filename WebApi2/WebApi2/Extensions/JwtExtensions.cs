using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace WebApi2.Extensions;

public static class JwtExtensions
{
    public static IServiceCollection AddGatewayAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication("GatewayAuth")
            .AddScheme<AuthenticationSchemeOptions, GatewayAuthHandler>("GatewayAuth", null);

        services.AddAuthorization();
        return services;
    }

    public static void AddSwaggerJwtAuth(this SwaggerGenOptions options)
    {
        // 1. Описываем, как именно передавать токен
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Введите ваш токен:",
        });

        //options.OperationFilter<AuthorizeCheckOperationFilter>();
        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
    }
}

public class GatewayAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public GatewayAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Ocelot пришлет имя в заголовке X-User-Name
        if (!Request.Headers.TryGetValue("X-User-Name", out var userName))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName.ToString()) };
        var identity = new ClaimsIdentity(claims, "GatewayAuth");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "GatewayAuth");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}