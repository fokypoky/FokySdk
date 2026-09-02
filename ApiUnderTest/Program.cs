using FokySdk.DataAccess;
using FokySdk.Types.Settings;
using ApiUnderTest.Consumers;
using FokySdk.Middlewares;
using FokySdk.Swagger;
using FokySdk.Types.DataAccess;
using FokySdk.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithNewtonsoft();
builder.Services.AddSwagger(new SwaggerSettings()
    { ServiceName = "ApiUnderTest", ServiceVersion = "v1", JwtAuthEnabled = true });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<VersioningMiddleware>();

app.UseHttpsRedirection();
app.AddSwagger(new SwaggerSettings() { ServiceName = "ApiUnderTest", ServiceVersion = "v1" });
app.UseAuthorization();

app.MapControllers();

app.Run();
