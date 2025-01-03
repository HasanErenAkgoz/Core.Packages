using Core.Packages.Application.Interfaces;
using Core.Packages.Application.Security.Attributes;
using Core.Packages.Domain.Security.Permissions.Entities;
using Core.Packages.Domain.Security.Permissions.Models;
using Core.Packages.Domain.Security.Permissions.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Core.Packages.Application.Security.Services;

public class PermissionDiscoveryManager : IPermissionDiscoveryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PermissionDiscoveryManager> _logger;
    private readonly IMemoryCache _cache;

    public PermissionDiscoveryManager(
        IUnitOfWork unitOfWork,
        ILogger<PermissionDiscoveryManager> logger,
        IMemoryCache cache)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _cache = cache;
    }

    public async Task DiscoverAndSavePermissionsAsync(Assembly assembly)
    {
        try
        {
            _logger.LogInformation("Permission discovery başlatıldı...");

            // Önce mevcut permission'ları al
            var existingPermissions = await _unitOfWork.Repository<Permission>()
                .Query()
                .Select(p => p.Name)
                .ToListAsync();

            // Command ve Query'leri bul
            var types = assembly.GetTypes()
                .Where(t => typeof(IRequest).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            var newPermissions = new List<Permission>();
            var updatedCount = 0;

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<RequiredPermissionAttribute>();
                var permission = attribute?.Permission ?? RequiredPermissionAttribute.GeneratePermission(type);
                
                // Eğer permission zaten varsa atla
                if (existingPermissions.Contains(permission))
                {
                    updatedCount++;
                    continue;
                }

                var groupName = permission.Split('.')[0]; // "Services.Create" -> "Services"
                var operationType = permission.Split('.')[1]; // "Services.Create" -> "Create"

                newPermissions.Add(new Permission
                {
                    Name = permission,
                    DisplayName = $"{groupName} {operationType}",
                    GroupName = $"{groupName} İşlemleri",
                    Description = $"{groupName} için {operationType} yetkisi"
                });
            }

            if (newPermissions.Any())
            {
                await _unitOfWork.AddRangeAsync(newPermissions);
                await _unitOfWork.SaveChangesAsync();

                // Cache'i temizle
                _cache.Remove("all_permissions");
            }

            _logger.LogInformation(
                "Permission discovery tamamlandı. Yeni: {NewCount}, Mevcut: {ExistingCount}", 
                newPermissions.Count, 
                updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Permission discovery sırasında hata oluştu");
            throw;
        }
    }

    public async Task<List<PermissionInfo>> GetDefaultPermissionsForRoleAsync(string roleName)
    {
        // Cache'den al, yoksa DB'den getir ve cache'e ekle
        var cacheKey = $"default_permissions_{roleName}";
        
        if (_cache.TryGetValue(cacheKey, out List<PermissionInfo> cachedPermissions))
            return cachedPermissions;

        var defaultPermissions = roleName.ToLower() switch
        {
            "service.manager" => new[] { "Create", "Read", "Update" },
            "technician" => new[] { "Read", "Update" },
            "receptionist" => new[] { "Read", "Create" },
            _ => Array.Empty<string>()
        };

        var allPermissions = await GetAllPermissionsFromDbAsync();

        var permissions = allPermissions
            .Where(p => defaultPermissions.Any(dp => p.Name.EndsWith($".{dp}")))
            .Select(p => new PermissionInfo
            {
                Key = p.Name,
                DisplayName = p.DisplayName,
                GroupName = p.GroupName,
                Description = p.Description
            })
            .ToList();

        _cache.Set(cacheKey, permissions, TimeSpan.FromHours(24));
        
        return permissions;
    }

    private async Task<List<Permission>> GetAllPermissionsFromDbAsync()
    {
        var cacheKey = "all_permissions";
        
        if (_cache.TryGetValue(cacheKey, out List<Permission> cachedPermissions))
            return cachedPermissions;

        var permissions = await _unitOfWork.Repository<Permission>()
            .Query()
            .ToListAsync();

        _cache.Set(cacheKey, permissions, TimeSpan.FromHours(24));
        
        return permissions;
    }

    public async Task SaveDefaultRolePermissionsAsync(string roleName, List<string> permissions)
    {
        foreach (var permission in permissions)
        {
            await _unitOfWork.Repository<RolePermission>().AddAsync(new RolePermission
            {
                RoleName = roleName,
                Permission = permission
            });
        }

        await _unitOfWork.SaveChangesAsync();
    }
} 