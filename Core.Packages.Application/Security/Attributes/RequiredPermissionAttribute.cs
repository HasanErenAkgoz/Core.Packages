using System.Text.RegularExpressions;
using MediatR;

namespace Core.Packages.Application.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiredPermissionAttribute : Attribute
    {
        public string Permission { get; }

        public RequiredPermissionAttribute(string permission = null)
        {
            Permission = permission;
        }

        public static string GeneratePermission<T>() where T : IRequest
        {
            var type = typeof(T);
            return GeneratePermission(type);
        }

        public static string GeneratePermission(Type type)
        {
            if (!typeof(IRequest).IsAssignableFrom(type))
                throw new ArgumentException("Type must implement IRequest", nameof(type));

            var className = type.Name;
            var namespaceParts = type.Namespace?.Split('.');

            // Entity adını bul (Features.{Entity}.Commands/Queries)
            var entityName = namespaceParts?.FirstOrDefault(p =>
                p != "Features" && p != "Commands" && p != "Queries");

            if (string.IsNullOrEmpty(entityName))
                throw new InvalidOperationException($"Could not determine entity name from namespace: {type.Namespace}");

            // Command/Query son ekini kaldır
            className = Regex.Replace(className, "(Command|Query)$", "");

            // İlk kelimeyi (action) al
            var action = Regex.Match(className, "^[A-Z][a-z]+").Value;
            if (string.IsNullOrEmpty(action))
                throw new InvalidOperationException($"Could not determine action from class name: {className}");

            return $"{entityName}.{action}";
        }
    }
}