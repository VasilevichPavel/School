using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using School.Core.Exceptions;
using System.Text.Json;

namespace School.Application.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                var response = new { Success = "True" };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                if (ex is NotFoundEntityException || ex is ValidationException)
                {
                    _logger.LogError(ex, ex.Message);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(ex.Message));
                }
                else
                {
                    _logger.LogError(ex, "An unhandled exception has occurred.");

                    var response = new { ex.Message, Error = ex.StackTrace };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            }
        }
    }
}
