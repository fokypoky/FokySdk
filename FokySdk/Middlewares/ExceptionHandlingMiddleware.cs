using System.Net;
using FokySdk.Types.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FokySdk.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public static string? InternalExceptionCode { get; set; } = null;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ApiException<ServiceError> ex)
            {
                await HandleException(ex, context);
            }
            catch (Exception ex)
            {
                await HandleException(ex, context);
            }
        }

        private async Task HandleException(Exception exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = new ServiceError()
            {
                Code = InternalExceptionCode,
                Reason = $"Internal server error. {exception.Message}"
            };
            var serialized = JsonConvert.SerializeObject(error, Formatting.Indented, Constants.Constants.SerializerSettings);

            await context.Response.WriteAsync(serialized);
        }

        private async Task HandleException(ApiException<ServiceError> exception, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            var serialized = JsonConvert.SerializeObject(exception.Result, Formatting.Indented, Constants.Constants.SerializerSettings);

            await context.Response.WriteAsync(serialized);
        }
    }
}
