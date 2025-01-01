using System.Reflection;
using Core.Packages.Domain.Security.Permissions.Models;

namespace Core.Packages.Domain.Security.Permissions.Services;

public interface IPermissionDiscoveryService
{
    Task DiscoverAndSavePermissionsAsync(Assembly assembly);
    Task<List<PermissionInfo>> GetDefaultPermissionsForRoleAsync(string roleName);
    Task SaveDefaultRolePermissionsAsync(string roleName, List<string> permissions);
} 