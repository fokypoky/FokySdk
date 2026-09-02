# 🚀 FokySdk

> 🧰 **Infrastructure SDK for .NET 10 microservices** — common integrations and conventions in one reusable package.

![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![NuGet](https://img.shields.io/badge/NuGet-FokySdk-004880?logo=nuget&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

✨ **FokySdk** is an infrastructure SDK for .NET 10 microservices. It
provides reusable integrations and conventions for:

-   Swagger / OpenAPI
-   JWT authorization support in Swagger UI
-   PostgreSQL / Entity Framework Core
-   RabbitMQ / MassTransit
-   OpenTelemetry
-   NLog-based logging
-   ASP.NET Core middleware
-   Controller response mapping
-   Newtonsoft.Json controller configuration
-   Service version headers

The package is designed to keep common infrastructure configuration out
of individual microservice projects.

## 📋 Requirements

-   .NET 10
-   ASP.NET Core 10

## 📦 Installation

Install the NuGet package:

``` bash
dotnet add package FokySdk
```

`FokySdk` depends on `FokySdk.Types`, which contains shared DTOs and
configuration types used by the SDK.

------------------------------------------------------------------------

---

# 📚 Swagger

Namespace:

``` csharp
using FokySdk.Swagger;
using FokySdk.Types.Settings;
```

## 🛠️ Register Swagger

``` csharp
builder.Services.AddSwagger(new SwaggerSettings
{
    ServiceName = "UsersService",
    ServiceVersion = "v1"
});
```

Then add Swagger middleware:

``` csharp
app.AddSwagger(new SwaggerSettings
{
    ServiceName = "UsersService",
    ServiceVersion = "v1"
});
```

This registers the Swagger document and Swagger UI.

## 🌱 Read service information from environment variables

`SwaggerSettings.GetFromEnvironment()` reads:

  Variable            Default     Description
  ------------------- ----------- --------------------------
  `SERVICE_NAME`      `SERVICE`   Swagger service title
  `SERVICE_VERSION`   `DEV`       Swagger document version

Example:

``` csharp
var swaggerSettings = SwaggerSettings.GetFromEnvironment();

builder.Services.AddSwagger(swaggerSettings);

// ...

app.AddSwagger(swaggerSettings);
```

## 🔐 JWT support in Swagger

JWT support can be enabled with:

``` csharp
var swaggerSettings = SwaggerSettings.GetFromEnvironment()
    .WithJwtAuthEnabled();

builder.Services.AddSwagger(swaggerSettings);

// ...

app.AddSwagger(swaggerSettings);
```

Or explicitly:

``` csharp
builder.Services.AddSwagger(new SwaggerSettings
{
    ServiceName = "UsersService",
    ServiceVersion = "v1",
    JwtAuthEnabled = true
});
```

When enabled, Swagger UI exposes a **Bearer JWT authorization** scheme
and adds the corresponding security requirement to the OpenAPI document.

This enables the **Authorize** button in Swagger UI and allows
authenticated requests to send:

``` http
Authorization: Bearer <jwt-token>
```

> `JwtAuthEnabled` configures JWT support in the Swagger/OpenAPI
> documentation. It does not configure ASP.NET Core JWT authentication
> itself. Actual JWT validation must still be configured by the
> application.

------------------------------------------------------------------------

---

# 🗄️ Entity Framework Core

Namespace:

``` csharp
using FokySdk.DataAccess;
using FokySdk.Types.Settings;
```

## 🔌 Register DbContext

``` csharp
builder.Services.AddEfDbContext<MyDbContext>(
    EfCoreConnectionSettings.GetFromEnvironment());
```

The SDK configures the context to use PostgreSQL through Npgsql.

## 🌱 Environment variables

`EfCoreConnectionSettings.GetFromEnvironment()` reads:

  Variable          Required Description
  --------------- ---------- -----------------------------------------
  `PG_HOST`              Yes PostgreSQL host
  `PG_PORT`              Yes PostgreSQL port
  `PG_USER`              Yes PostgreSQL username
  `PG_PASSWORD`          Yes PostgreSQL password
  `PG_DATABASE`          Yes Database name
  `PG_SCHEMA`             No PostgreSQL schema; defaults to `public`

The generated connection string also enables detailed PostgreSQL errors
and sets the configured search path.

------------------------------------------------------------------------

---

# 🐇 RabbitMQ

Namespace:

``` csharp
using FokySdk.DataAccess;
using FokySdk.Types.DataAccess;
using FokySdk.Types.Settings;
```

RabbitMQ integration is built on top of MassTransit.

## 🔌 Connection settings

``` csharp
var rabbitSettings = RabbitMqSettings.GetFromEnvironment();
```

Environment variables:

  Variable                 Required Default
  ---------------------- ---------- ---------
  `RABBIT_MQ_HOST`              Yes ---
  `RABBIT_MQ_PORT`              Yes ---
  `RABBIT_MQ_USER`              Yes ---
  `RABBIT_MQ_PASSWORD`          Yes ---
  `RABBIT_MQ_VHOST`              No `/`

## 🛠️ Register RabbitMQ

``` csharp
builder.Services.AddRabbitMq(
    rabbitSettings,
    consumersRegister: x =>
    {
        x.AddConsumer<UserCreatedConsumer>();
    },
    consumersAdd: (cfg, context) =>
    {
        RabbitMq.AddConsumer<UserCreatedConsumer>(
            cfg,
            context,
            new RabbitMqConsumer
            {
                Queue = "users-service",
                Exchange = "users",
                RoutingKey = "user.created",
                ExchangeType = ExchangeType.Topic
            });
    },
    publishersRegister: cfg =>
    {
        RabbitMq.AddPublisher<UserCreated>(
            cfg,
            new RabbitMqPublisher("users"));
    });
```

All consumer/publisher registration callbacks are optional and can be
`null`.

## 📥 Consumer

`RabbitMqConsumer` describes a consumer endpoint:

``` csharp
new RabbitMqConsumer
{
    Queue = "users-service",
    Exchange = "users",
    RoutingKey = "user.created",
    ExchangeType = ExchangeType.Topic
}
```

The SDK creates a receive endpoint, disables MassTransit's automatic
consume topology, binds the specified exchange, and configures the
consumer.

## 🔄 Retry

A consumer can optionally use a retry policy:

``` csharp
RabbitMq.AddConsumer<UserCreatedConsumer>(
    cfg,
    context,
    consumer,
    new RabbitMqRetrySettings
    {
        RetryCount = 3,
        Interval = TimeSpan.FromSeconds(5)
    });
```

This configures an interval retry policy.

## 📤 Publisher

Publishers can be configured with:

``` csharp
new RabbitMqPublisher(
    exchange: "users",
    exchangeType: ExchangeType.Topic,
    durable: true);
```

Then:

``` csharp
RabbitMq.AddPublisher<UserCreated>(cfg, publisher);
```

The SDK configures the MassTransit message entity name, durability, and
exchange type.

Currently supported exchange type:

``` csharp
ExchangeType.Topic
```

------------------------------------------------------------------------

---

# 📈 OpenTelemetry

Namespace:

``` csharp
using FokySdk.Telemetry;
using FokySdk.Types.Settings;
```

## 🛠️ Register OpenTelemetry

``` csharp
var serviceInfo = OtelServiceInfo.GetFromEnvironment();

builder.Services.AddOtelServices(
    serviceInfo,
    new OtelSettings
    {
        UseAspNetCoreInstrumentation = true,
        UseHttpClientInstrumentation = true,
        UseEntityFrameworkInstrumentation = true,
        UseMassTransitInstrumentation = true
    });
```

The SDK configures:

-   OpenTelemetry tracing
-   ASP.NET Core instrumentation
-   HttpClient instrumentation
-   Entity Framework Core instrumentation
-   MassTransit instrumentation
-   OTLP/gRPC exporting
-   W3C Trace Context propagation
-   Baggage propagation

Each instrumentation type can be enabled independently.

## 🌱 Environment variables

`OtelServiceInfo.GetFromEnvironment()` reads:

  Variable                 Required Description
  ---------------------- ---------- -----------------------------
  `OTEL_SERVICE_NAME`           Yes Service name
  `OTEL_GRPC_ENDPOINT`          Yes OTLP gRPC exporter endpoint

Example:

``` text
OTEL_SERVICE_NAME=users-service
OTEL_GRPC_ENDPOINT=http://jaeger-collector:4317
```

## 🧩 Custom ActivitySource

The SDK exposes a shared `ActivitySource`:

``` csharp
using FokySdk.Telemetry;

var activitySource = OpenTelemetry.Providers.TraceSource;
```

It is registered in dependency injection as a singleton.

You can create custom spans using the registered `ActivitySource`.

------------------------------------------------------------------------

---

# 📝 Logging

Namespace:

``` csharp
using FokySdk.Logging;
using FokySdk.Types.Settings;
```

The SDK provides an `ILogger` abstraction backed by NLog.

## ⚙️ Configure logger

``` csharp
var settings = new LoggerSettings
{
    UseConsoleTarget = true,
    UseFileTarget = true,
    FileName = "app.log",
    MinLevel = NLog.LogLevel.Info,
    MaxLevel = NLog.LogLevel.Fatal
};

var logger = new Logger(settings);
```

`LoggerSettings` supports:

  ------------------------------------------------------------------------------------------------
  Property                Default                                          Description
  ----------------------- ------------------------------------------------ -----------------------
  `ConsoleLayout`         `${time} ${level} ${message}`                    Console log layout

  `FileLayout`            `${longdate} ${level} ${message} ${exception}`   File log layout

  `FileName`              `app.log`                                        File target name/path

  `UseConsoleTarget`      `true`                                           Enable console target

  `ConsoleTargetName`     `console`                                        Console target name

  `UseFileTarget`         `true`                                           Enable file target

  `FileTargetName`        `file`                                           File target name

  `MinLevel`              `Info`                                           Minimum log level

  `MaxLevel`              `Fatal`                                          Maximum log level

  `ExcludedStrings`       ---                                              Strings to mask in
                                                                           messages
  ------------------------------------------------------------------------------------------------

Sensitive strings can be masked before the message is written:

``` csharp
new LoggerSettings
{
    ExcludedStrings = new List<string>
    {
        "secret-password",
        "sensitive-token"
    }
};
```

The logger replaces each excluded string with `*`.

## 🪵 ILogger

``` csharp
public interface ILogger
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message);
}
```

------------------------------------------------------------------------

---

# 🎯 Controller response mapping

Namespace:

``` csharp
using FokySdk.Controller;
```

The SDK provides `MapResponse` extension methods for converting
`ServiceResult<T>` into ASP.NET Core `ActionResult`.

## 📡 Basic response

``` csharp
[HttpGet]
public IActionResult Get()
{
    var result = ServiceResult<MyDto>.Ok(data);

    return this.MapResponse(result);
}
```

The following statuses are mapped:

  `ResultStatus`     HTTP response
  ------------------ -----------------------------
  `Ok`               `200 OK`
  `Created`          `201 Created`
  `NoContent`        `204 No Content`
  `PartialContent`   `206 Partial Content`
  `BadRequest`       `400 Bad Request`
  `NotFound`         `404 Not Found`
  `InternalError`    `500 Internal Server Error`

Error responses are converted to `ServiceError`.

## 📄 Paginated response

For:

``` csharp
ServiceResult<PaginatedResponse<T>>
```

the SDK additionally sets:

``` http
x-total-count: <total-count>
```

and returns the paginated data collection as the response body.

Example:

``` csharp
var result =
    ServiceResult<PaginatedResponse<MyDto>>.Ok(
        new PaginatedResponse<MyDto>
        {
            Data = items,
            TotalCount = totalCount
        });

return this.MapResponse(result);
```

------------------------------------------------------------------------

---

# 🌐 ASP.NET Core Web API

Namespace:

``` csharp
using FokySdk.WebApi;
```

## 🔧 Controllers with Newtonsoft.Json

``` csharp
builder.Services.AddControllersWithNewtonsoft();
```

This registers controllers and configures Newtonsoft.Json with the SDK's
standard serializer settings:

-   indented JSON
-   ignored null values
-   UTC date format: `yyyy-MM-ddTHH:mm:ssZ`

------------------------------------------------------------------------

---

# 🧱 Middleware

The SDK contains several reusable ASP.NET Core middleware components.

Namespace:

``` csharp
using FokySdk.Middlewares;
```

## 📦 BufferingMiddleware

``` csharp
app.UseMiddleware<BufferingMiddleware>();
```

Enables request body buffering so the request body can be read by
middleware and subsequently consumed by the endpoint.

This is useful when request logging needs to inspect the body.

## 📝 LoggingMiddleware

``` csharp
app.UseMiddleware<LoggingMiddleware>();
```

Logs HTTP method, request path, and request body through the SDK logger.

Swagger requests are excluded from body logging.

`LoggingMiddleware` expects `FokySdk.Logging.ILogger` to be registered
in dependency injection.

## 🏷️ VersioningMiddleware

``` csharp
app.UseMiddleware<VersioningMiddleware>();
```

Adds the following response header:

``` http
x-service-version: <SERVICE_VERSION>
```

If `SERVICE_VERSION` is not defined, the value is:

``` text
DEV
```

## 🚨 ExceptionHandlingMiddleware

``` csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

Provides centralized exception handling.

`ApiException<ServiceError>` responses are returned using the
exception's status code and `ServiceError`.

Unexpected exceptions are returned as:

``` http
500 Internal Server Error
Content-Type: application/json
```

An optional internal exception code can be configured:

``` csharp
ExceptionHandlingMiddleware.InternalExceptionCode = "INTERNAL_ERROR";
```

------------------------------------------------------------------------

---

# 💡 Example application

A typical microservice can combine the SDK components like this:

``` csharp
using FokySdk.Controller;
using FokySdk.DataAccess;
using FokySdk.Middlewares;
using FokySdk.Swagger;
using FokySdk.Telemetry;
using FokySdk.Types.Settings;
using FokySdk.WebApi;

var builder = WebApplication.CreateBuilder(args);

var swaggerSettings = SwaggerSettings.GetFromEnvironment()
    .WithJwtAuthEnabled();

builder.Services.AddControllersWithNewtonsoft();

builder.Services.AddSwagger(swaggerSettings);

builder.Services.AddEfDbContext<MyDbContext>(
    EfCoreConnectionSettings.GetFromEnvironment());

builder.Services.AddOtelServices(
    OtelServiceInfo.GetFromEnvironment(),
    new OtelSettings
    {
        UseAspNetCoreInstrumentation = true,
        UseHttpClientInstrumentation = true,
        UseEntityFrameworkInstrumentation = true,
        UseMassTransitInstrumentation = true
    });

builder.Services.AddRabbitMq(
    RabbitMqSettings.GetFromEnvironment(),
    consumersRegister: null,
    consumersAdd: null,
    publishersRegister: null);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<VersioningMiddleware>();
app.UseMiddleware<BufferingMiddleware>();

app.AddSwagger(swaggerSettings);

app.UseAuthorization();

app.MapControllers();

app.Run();
```

------------------------------------------------------------------------

---

# 📦 Package dependencies

The package currently uses the following major dependencies:

-   `FokySdk.Types` 1.4.0
-   `MassTransit` 8.5.10
-   `MassTransit.RabbitMQ` 8.5.10
-   `Microsoft.AspNetCore.Mvc.NewtonsoftJson` 10.0.9
-   `Microsoft.Extensions.DependencyInjection.Abstractions` 10.0.9
-   `Microsoft.OpenApi` 2.12.2
-   `Newtonsoft.Json` 13.0.4
-   `NLog` 6.1.4
-   `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3
-   OpenTelemetry 1.17.0
-   `OpenTelemetry.Instrumentation.EntityFrameworkCore` 1.17.0-beta.1
-   `OpenTelemetry.Instrumentation.MassTransit` 1.0.0-beta.3
-   Swashbuckle ASP.NET Core 10.2.3

------------------------------------------------------------------------

---

# 🔨 Building the package

Build the project:

``` bash
dotnet build -c Release
```

Create a NuGet package:

``` bash
dotnet pack ./FokySdk/FokySdk.csproj -c Release -o ./nupkgs
```

The project is configured with:

``` xml
<GeneratePackageOnBuild>true</GeneratePackageOnBuild>
```

so a package is also generated during a Release build.

------------------------------------------------------------------------

---

# 📜 License

MIT
