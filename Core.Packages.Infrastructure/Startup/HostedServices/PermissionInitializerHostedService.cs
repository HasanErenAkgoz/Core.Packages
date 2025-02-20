using Core.Packages.Application.Features.Permissions.Commands.ScanAndRegister;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Packages.Infrastructure.Startup.HostedServices
{
    public class PermissionInitializerHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public PermissionInitializerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new ScanAndRegisterPermissionsCommand(), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}
