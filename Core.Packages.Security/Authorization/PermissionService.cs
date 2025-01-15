using Core.Packages.Application.Interfaces;
using Core.Packages.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Security.Authorization;

public class PermissionService : IPermissionService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public PermissionService(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var hasPermission = role.RolePermissions
                .Any(rp => rp.Permission == permission);

            if (hasPermission) return true;
        }

        return false;
    }

    public async Task<bool> HasPermissionAsync(string userId, string[] permissions, bool requireAll = false)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var userRoles = await _userManager.GetRolesAsync(user);
        var userPermissions = new HashSet<string>();

        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var rolePermissions = role.RolePermissions.Select(rp => rp.Permission);
            userPermissions.UnionWith(rolePermissions);
        }

        if (requireAll)
            return permissions.All(p => userPermissions.Contains(p));
        
        return permissions.Any(p => userPermissions.Contains(p));
    }

    public async Task<IList<string>> GetUserPermissionsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new HashSet<string>();

        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var rolePermissions = role.RolePermissions.Select(rp => rp.Permission);
            permissions.UnionWith(rolePermissions);
        }

        return permissions.ToList();
    }

    public async Task<bool> AddPermissionToRoleAsync(string roleName, string permission)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return false;

        if (role.RolePermissions.Any(rp => rp.Permission == permission))
            return true;

        role.RolePermissions.Add(new RolePermission(role.Id, permission));
        var result = await _roleManager.UpdateAsync(role);

        return result.Succeeded;
    }

    public async Task<bool> RemovePermissionFromRoleAsync(string roleName, string permission)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return false;

        var rolePermission = role.RolePermissions.FirstOrDefault(rp => rp.Permission == permission);
        if (rolePermission == null) return true;

        role.RolePermissions.Remove(rolePermission);
        var result = await _roleManager.UpdateAsync(role);

        return result.Succeeded;
    }

    public async Task<IList<string>> GetRolePermissionsAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return new List<string>();

        return role.RolePermissions.Select(rp => rp.Permission).ToList();
    }
} 