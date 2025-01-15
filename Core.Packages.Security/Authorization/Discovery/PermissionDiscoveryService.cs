using System.Reflection;
using Core.Packages.Security.Authorization.Attributes;
using Core.Packages.Security.OAuth.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Packages.Security.Authorization.Discovery;

public class PermissionDiscoveryService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionDiscoveryService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var oAuthManager = scope.ServiceProvider.GetRequiredService<IOAuthManager>();

        // Tüm assembly'leri tara
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var permissions = new HashSet<string>();

        foreach (var assembly in assemblies)
        {
            try
            {
                // Handler'ları bul
                var handlers = assembly.GetTypes()
                    .Where(t => t.GetCustomAttribute<HandlerAuthorizationAttribute>() != null);

                foreach (var handler in handlers)
                {
                    var attribute = handler.GetCustomAttribute<HandlerAuthorizationAttribute>();
                    if (attribute?.Permission != null)
                    {
                        permissions.Add(attribute.Permission);
                    }
                }
            }
            catch (ReflectionTypeLoadException)
            {
                // Assembly taranamadıysa devam et
                continue;
            }
        }

        // Bulunan permission'ları veritabanına ekle
        if (permissions.Any())
        {
            await SyncPermissionsWithDatabase(oAuthManager, permissions, cancellationToken);
        }
    }

    private async Task SyncPermissionsWithDatabase(
        IOAuthManager oAuthManager, 
        HashSet<string> discoveredPermissions,
        CancellationToken cancellationToken)
    {
        // Sistemdeki tüm client'ları al
        var clients = await oAuthManager.GetAllClientsAsync(cancellationToken);
        
        // Admin client'ı bul veya oluştur
        var adminClient = clients.FirstOrDefault(c => c.Name == "AdminClient") ?? 
            await oAuthManager.CreateClientAsync(new()
            {
                Name = "AdminClient",
                Description = "Sistem yönetici client'ı",
                ClientId = "admin",
                ClientSecret = Guid.NewGuid().ToString("N"),
                IsActive = true
            }, cancellationToken);

        // Admin client'a tüm permission'ları ekle
        var currentScopes = adminClient.AllowedScopes.ToHashSet();
        var newScopes = discoveredPermissions.Except(currentScopes);

        if (newScopes.Any())
        {
            adminClient.AllowedScopes.AddRange(newScopes);
            await oAuthManager.UpdateClientAsync(adminClient, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
} 