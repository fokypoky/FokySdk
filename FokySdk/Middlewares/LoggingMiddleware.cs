using FokySdk.Logging;
using Microsoft.AspNetCore.Http;

namespace FokySdk.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.Value?.Contains("swagger", StringComparison.InvariantCultureIgnoreCase) ?? true)
            {
                var body = await ReadBody(context);
                _logger.LogInfo($"{context.Request.Method} {context.Request.Path}. Body: {body}");
            }

            await _next(context);
        }

        private async Task<string> ReadBody(HttpContext context)
        {
            context.Request.Body.Position = 0;

            var stream = context.Request.Body;
            using var reader = new StreamReader(stream, leaveOpen: true);

            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            return body;
        }
    }
}
