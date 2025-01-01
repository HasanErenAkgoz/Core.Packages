using Core.Packages.Application.Security.JWT;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Core.Packages.Infrastructure.Middleware;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;

    public TokenValidationMiddleware(RequestDelegate next, ITokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var isValid = await _tokenService.ValidateToken(token);
            if (!isValid)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(new { message = "Invalid or blacklisted token" });
                await context.Response.WriteAsync(result);
                return;
            }
        }

        await _next(context);
    }
}