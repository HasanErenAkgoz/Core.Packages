using System.Security.Claims;
using System.Reflection;
using Core.Packages.Application.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Core.Packages.Domain.Security.Permissions.Attributes;
using RequiredPermissionAttribute = Core.Packages.Domain.Security.Permissions.Attributes.RequiredPermissionAttribute;

namespace Core.Packages.Security.Permissions.PipelineBehaviors
{
    public class PermissionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRolePermissionService _rolePermissionService;

        public PermissionBehavior(
            IHttpContextAccessor httpContextAccessor,
            IRolePermissionService rolePermissionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var type = request.GetType();
            var permissionAttributes = type.GetCustomAttributes(typeof(RequiredPermissionAttribute), true)
                .Cast<RequiredPermissionAttribute>();

            if (!permissionAttributes.Any())
            {
                // Eğer attribute yoksa, otomatik permission oluştur
                var autoPermission = RequiredPermissionAttribute.GeneratePermission<TRequest>();
                var userRoles = GetUserRoles();

                if (!await _rolePermissionService.HasPermissionAsync(userRoles, autoPermission))
                    throw new UnauthorizedAccessException($"Bu işlem için gerekli izniniz bulunmamaktadır. Gerekli izin: {autoPermission}");
            }
            else
            {
                // Attribute varsa, belirtilen permission'ları kontrol et
                foreach (var attr in permissionAttributes)
                {
                    var permission = attr.Permission ?? RequiredPermissionAttribute.GeneratePermission<TRequest>();
                    var userRoles = GetUserRoles();

                    if (!await _rolePermissionService.HasPermissionAsync(userRoles, permission))
                        throw new UnauthorizedAccessException($"Bu işlem için gerekli izniniz bulunmamaktadır. Gerekli izin: {permission}");
                }
            }

            return await next();
        }

        private List<string> GetUserRoles()
        {
            return _httpContextAccessor.HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();
        }
    }
} 