using System.Reflection;
using Core.Packages.Application.Authorization;
using Core.Packages.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Core.Packages.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRolePermissionService _rolePermissionService;

    public AuthorizationBehavior(
        IHttpContextAccessor httpContextAccessor,
        IRolePermissionService rolePermissionService)
    {
        _httpContextAccessor = httpContextAccessor;
        _rolePermissionService = rolePermissionService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // RequiredPermissionAttribute'u kontrol et
        var requiredPermissionAttribute = request.GetType().GetCustomAttribute<RequiredPermissionAttribute>();
        
        // RequiredPermissionAttribute yoksa direkt geç
        if (requiredPermissionAttribute == null)
            return await next();

        // Kullanıcı authenticate olmamışsa hata fırlat
        if (!_httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? true)
            throw new UnauthorizedAccessException("Bu işlem için giriş yapmanız gerekiyor.");

        // Kullanıcının rollerini al
        var userRoles = _httpContextAccessor.HttpContext.User.Claims
            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        // Kullanıcının gerekli yetkiye sahip olup olmadığını kontrol et
        var hasPermission = await _rolePermissionService.HasPermissionAsync(userRoles, requiredPermissionAttribute.Permission);
        
        if (!hasPermission)
            throw new UnauthorizedAccessException($"Bu işlem için gerekli yetkiye sahip değilsiniz. Gerekli yetki: {requiredPermissionAttribute.Permission}");

        return await next();
    }
} 