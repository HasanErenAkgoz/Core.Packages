using Core.Packages.Security.Authorization.Attributes;
using Core.Packages.Security.OAuth.Services;
using MediatR;
using System.Reflection;

namespace Core.Packages.Security.Authorization.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IOAuthManager _oAuthManager;

    public AuthorizationBehavior(IOAuthManager oAuthManager)
    {
        _oAuthManager = oAuthManager;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var handlerType = request.GetType();
        var attribute = handlerType.GetCustomAttribute<HandlerAuthorizationAttribute>();

        if (attribute == null)
            return await next();

        var clientId = _oAuthManager.GetCurrentClientId();
        if (clientId == null)
            throw new UnauthorizedAccessException("Client kimliği bulunamadı.");

        var client = await _oAuthManager.GetClientByIdAsync(clientId);
        if (client == null)
            throw new UnauthorizedAccessException("Client bulunamadı.");

        var requiredPermission = attribute.Permission;
        var hasPermission = client.AllowedScopes.Contains(requiredPermission);

        if (!hasPermission)
            throw new UnauthorizedAccessException($"Bu işlem için yetkiniz bulunmamaktadır. Gerekli yetki: {requiredPermission}");

        return await next();
    }
} 