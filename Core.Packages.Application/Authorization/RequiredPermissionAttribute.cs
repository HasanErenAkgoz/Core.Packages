using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace Core.Packages.Application.Authorization;

public class RequiredPermissionAttribute : AuthorizeAttribute
{
    public string Permission { get; }

    public RequiredPermissionAttribute(string? permission = null)
    {
        Permission = permission ?? GeneratePermissionFromClassName();
        Policy = Permission;
    }

    private string GeneratePermissionFromClassName()
    {
        var type = GetType().DeclaringType;
        if (type == null) return string.Empty;

        var className = type.Name;
        var namespaceParts = type.Namespace?.Split('.');
        
        // Entity adını bul (Features.{Entity}.Commands/Queries)
        var entityName = namespaceParts?.FirstOrDefault(p => 
            p != "Features" && p != "Commands" && p != "Queries");

        // Command/Query son ekini kaldır
        className = Regex.Replace(className, "(Command|Query)$", "");
        
        // İlk kelimeyi (action) al
        var action = Regex.Match(className, "^[A-Z][a-z]+").Value;

        return $"{entityName}.{action}";
    }
} 