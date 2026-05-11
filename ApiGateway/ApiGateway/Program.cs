using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Относительный путь позволит браузеру правильно собрать URL
    c.SwaggerEndpoint("/web-api-1/swagger/v1/swagger.json", "WebApi-1 service");
    c.SwaggerEndpoint("/web-api-2/swagger/v1/swagger.json", "WebApi-2 service");
});
app.UseForwardedHeaders(); 
await app.UseOcelot();

app.Run();