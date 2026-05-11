using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 1. Прямой доступ
    c.AddServer(new OpenApiServer { Url = "http://localhost:7000", Description = "Direct Access" });
    // 2. Доступ через Gateway
    c.AddServer(new OpenApiServer { Url = "http://localhost:4000/web-api-2", Description = "Via Gateway" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(); // По умолчанию доступно по адресу /swagger
}

app.MapControllers();
app.Run();