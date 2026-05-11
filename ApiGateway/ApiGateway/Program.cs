using ApiGateway.Extensions;
using Microsoft.OpenApi;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOcelot();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway API", Version = "v1" });
    c.AddSwaggerJwtAuth();
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var actionNamespace = apiDesc.ActionDescriptor.RouteValues["controller"];
        // Если контроллер из пространства имен Ocelot — игнорируем его
        return !apiDesc.RelativePath!.StartsWith("configuration") &&
            !apiDesc.RelativePath!.StartsWith("outputcache");
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway API (Auth)");
    // Относительный путь позволит браузеру правильно собрать URL
    c.SwaggerEndpoint("/web-api-1/swagger/v1/swagger.json", "WebApi-1 service");
    c.SwaggerEndpoint("/web-api-2/swagger/v1/swagger.json", "WebApi-2 service");
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // ПРАВИЛО ДЛЯ ЛОКАЛЬНЫХ КОНТРОЛЛЕРОВ ШЛЮЗА
});

//app.UseForwardedHeaders();
await app.UseOcelot();

app.Run();