using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security;
using System.Text.Json;

namespace Core.Packages.Persistence.Middlewares
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

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.LogError($"Hata yakalandı: {e.Message}");

                if (!httpContext.Response.HasStarted) // ✅ Yanıt başladı mı kontrol et
                {
                    await HandleExceptionAsync(httpContext, e);
                }
                else
                {
                    _logger.LogWarning("Yanıt başlatıldığı için hata mesajı yazılamadı.");
                }
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = e switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                ApplicationException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                SecurityException => StatusCodes.Status401Unauthorized,
                NotSupportedException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                httpContext.Response.StatusCode,
                Message = e.Message ?? "Bilinmeyen bir hata oluştu.",
                Timestamp = DateTime.UtcNow
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(jsonResponse);
        }
    }
}