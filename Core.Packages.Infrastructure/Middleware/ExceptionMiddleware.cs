using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Packages.Infrastructure.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                string.Join("; ", validationException.Errors.Select(x => x.ErrorMessage))
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Bu işlem için yetkiniz bulunmamaktadır."
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                "Beklenmeyen bir hata oluştu."
            )
        };

        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = (int)statusCode,
            Message = message,
            Success = false
        });

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
} 