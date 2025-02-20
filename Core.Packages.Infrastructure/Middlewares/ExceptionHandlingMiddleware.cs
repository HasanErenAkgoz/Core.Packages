using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Core.Packages.Infrastructure.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                // Özel exception işleme
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                // Genel exception işleme
                await HandleExceptionAsync(context, new CustomException("An unexpected error occurred."));
            }
        }

        private Task HandleExceptionAsync(HttpContext context, CustomException ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { message = ex.Message });
            return context.Response.WriteAsync(result);
        }
    }
}
