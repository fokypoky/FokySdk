using Microsoft.AspNetCore.Http;

namespace FokySdk.Middlewares
{
    public class VersioningMiddleware
    {
        private readonly RequestDelegate _next;

        public VersioningMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            SetVersion(context);
            await _next(context);
        }

        public void SetVersion(HttpContext context)
        {
            var version = Environment.GetEnvironmentVariable("SERVICE_VERSION") ?? "DEV";
            context.Response.Headers.TryAdd("x-service-version", version);
        }
    }
}