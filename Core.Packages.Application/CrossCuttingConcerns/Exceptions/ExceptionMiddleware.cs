using Core.Packages.CrossCuttingConcerns.Exceptions.HttpProblemDetails;
using Core.Packages.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ValidationProblemDetails = Core.Packages.CrossCuttingConcerns.Exceptions.HttpProblemDetails.ValidationProblemDetails;

namespace Core.Packages.CrossCuttingConcerns.Exceptions;

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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        object response = exception switch
        {
            BusinessException businessException => new BusinessProblemDetails(businessException.Message),
            
            ValidationException validationException => new ValidationProblemDetails(validationException.Errors),
            
            AuthorizationException authorizationException => new AuthorizationProblemDetails(authorizationException.Message),
            
            _ => new ProblemDetails
            {
                Title = "Internal server error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://example.com/probs/internal"
            }
        };

        context.Response.StatusCode = (response as ProblemDetails)?.Status ?? StatusCodes.Status500InternalServerError;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
} 