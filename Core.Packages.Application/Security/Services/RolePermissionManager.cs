using Core.Packages.Application.Interfaces;
using Core.Packages.Domain.Security.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public class RolePermissionManager : IRolePermissionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private const string CacheKeyPrefix = "role_permissions_";

    public RolePermissionManager(IUnitOfWork unitOfWork, IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<bool> HasPermissionAsync(IEnumerable<string> userRoles, string permission)
    {
        foreach (var role in userRoles)
        {
            var permissions = await GetPermissionsAsync(role);
            if (permissions.Contains(permission))
                return true;
        }

        return false;
    }

    public async Task<IEnumerable<string>> GetPermissionsAsync(string roleName)
    {
        var cacheKey = $"{CacheKeyPrefix}{roleName}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<string> permissions))
            return permissions;

        permissions = await _unitOfWork.Repository<RolePermission>()
            .Query()
            .Where(rp => rp.RoleName == roleName)
            .Select(rp => rp.Permission)
            .ToListAsync();

        _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(30));

        return permissions;
    }

    public async Task AddPermissionToRoleAsync(string roleName, string permission)
    {
        var rolePermission = new RolePermission { RoleName = roleName, Permission = permission };
        await _unitOfWork.Repository<RolePermission>().AddAsync(rolePermission);
        await _unitOfWork.CommitTransactionAsync();
        _cache.Remove($"{CacheKeyPrefix}{roleName}");
    }

    public async Task RemovePermissionFromRoleAsync(string roleName, string permission)
    {
        var rolePermission = await _unitOfWork.Repository<RolePermission>()
            .Query()
            .FirstOrDefaultAsync(rp => rp.RoleName == roleName && rp.Permission == permission);

        if (rolePermission != null)
        {
            await _unitOfWork.Repository<RolePermission>().DeleteAsync(rolePermission);
            await _unitOfWork.CommitTransactionAsync();
            _cache.Remove($"{CacheKeyPrefix}{roleName}");
        }
    }
}