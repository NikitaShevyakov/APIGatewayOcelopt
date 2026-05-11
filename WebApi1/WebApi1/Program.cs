using Microsoft.OpenApi;
using WebApi1.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddGatewayAuthentication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSwaggerJwtAuth();
    c.AddServer(new OpenApiServer { Url = "/web-api-1", Description = "Via Gateway" });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); // По умолчанию доступно по адресу /swagger
}

app.MapControllers();
app.Run();