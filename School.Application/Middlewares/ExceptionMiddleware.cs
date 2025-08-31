using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace School.Application.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;

        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new { ex.Message, Error = ex.StackTrace };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
