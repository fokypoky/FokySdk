using FokySdk.DataAccess;
using FokySdk.Types.Settings;
using ApiUnderTest.Consumers;
using FokySdk.Swagger;
using FokySdk.Types.DataAccess;
using FokySdk.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithNewtonsoft();
builder.Services.AddSwagger(new SwaggerSettings() { ServiceName = "ApiUnderTest", ServiceVersion = "v1" });
builder.Services.AddRabbitMq(RabbitMqSettings.GetFromEnvironment(),
    (factory) =>
    {
        factory.AddConsumer<UserCreatedConsumer>();
    },
    (factory, context) =>
{
    RabbitMq.AddConsumer<UserCreatedConsumer>(factory, context, new RabbitMqConsumer()
    {
        Exchange = "users",
        ExchangeType = ExchangeType.Topic,
        Queue = "user.created.lobby_service",
        RoutingKey = "user.created"
    });
}, null);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.AddSwagger(new SwaggerSettings() { ServiceName = "ApiUnderTest", ServiceVersion = "v1" });
app.UseAuthorization();

app.MapControllers();

app.Run();
