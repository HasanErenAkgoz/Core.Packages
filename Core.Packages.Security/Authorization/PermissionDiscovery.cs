using System.Reflection;
using MediatR;

namespace Core.Packages.Security.Authorization;

public static class PermissionDiscovery
{
    public static IEnumerable<string> DiscoverPermissions(params Assembly[] assemblies)
    {
        var permissions = new HashSet<string>();

        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && 
                        (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                         i.GetGenericTypeDefinition() == typeof(IRequestHandler<>))));

            foreach (var handlerType in handlerTypes)
            {
                var moduleName = GetModuleName(handlerType);
                var actionName = GetActionName(handlerType);

                if (!string.IsNullOrEmpty(moduleName) && !string.IsNullOrEmpty(actionName))
                {
                    permissions.Add($"Permissions.{moduleName}.{actionName}");
                }
            }
        }

        return permissions;
    }

    private static string GetModuleName(Type handlerType)
    {
        var name = handlerType.Name;
        
        if (name.EndsWith("CommandHandler") || name.EndsWith("QueryHandler"))
        {
            name = name.Replace("CommandHandler", "")
                      .Replace("QueryHandler", "");

            var prefixes = new[] { "Create", "Update", "Delete", "Get", "List" };
            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix))
                {
                    name = name.Substring(prefix.Length);
                    break;
                }
            }
        }

        return name;
    }

    private static string GetActionName(Type handlerType)
    {
        var name = handlerType.Name;

        if (name.Contains("Command") || name.Contains("Query"))
        {
            if (name.StartsWith("Create")) return "Create";
            if (name.StartsWith("Update")) return "Update";
            if (name.StartsWith("Delete")) return "Delete";
            if (name.StartsWith("Get")) return "View";
            if (name.StartsWith("List")) return "View";
        }

        return string.Empty;
    }
} 