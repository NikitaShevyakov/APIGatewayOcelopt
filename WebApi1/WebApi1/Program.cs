using Microsoft.OpenApi;
using WebApi1.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // c.AddServer(new OpenApiServer { Url = "/api1" });
    // 1. Прямой доступ
    c.AddServer(new OpenApiServer { Url = "http://localhost:5000", Description = "Direct Access" });
    // 2. Доступ через Gateway
    c.AddServer(new OpenApiServer { Url = "http://localhost:4000/api1", Description = "Via Gateway" });

    // c.AddSwaggerJwtAuth();
});

var app = builder.Build();

// app.UseAuthentication();
// app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); // По умолчанию доступно по адресу /swagger
}

app.MapControllers();
app.Run();