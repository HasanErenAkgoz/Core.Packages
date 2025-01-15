using System.Text.RegularExpressions;

namespace Core.Packages.Security.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HandlerAuthorizationAttribute : Attribute
{
    public string Permission { get; }

    public HandlerAuthorizationAttribute()
    {
        // Attribute'un kullanıldığı sınıfın adını alacağız
        var handlerType = GetType().DeclaringType;
        if (handlerType == null) 
            throw new ArgumentException("Attribute bir handler sınıfında kullanılmalıdır.");

        var handlerName = handlerType.Name;
        Permission = GeneratePermissionFromHandlerName(handlerName);
    }

    private static string GeneratePermissionFromHandlerName(string handlerName)
    {
        // "CreateProductCommandHandler" -> "product.create"
        // "UpdateUserCommandHandler" -> "user.update"
        // "DeleteOrderQueryHandler" -> "order.delete"
        
        // Command/Query kısmını kaldır
        handlerName = handlerName.Replace("CommandHandler", "")
                                .Replace("QueryHandler", "");

        // Camel case'i nokta ve küçük harfe çevir
        var permission = Regex.Replace(handlerName, "([a-z])([A-Z])", "$1.$2").ToLower();

        // İlk kelimeyi sona al (Create.Product -> product.create)
        var parts = permission.Split('.');
        if (parts.Length >= 2)
        {
            var entity = parts[^1];  // Son kelime (entity)
            var action = parts[0];   // İlk kelime (action)
            permission = $"{entity}.{action}";
        }

        return permission;
    }
} 