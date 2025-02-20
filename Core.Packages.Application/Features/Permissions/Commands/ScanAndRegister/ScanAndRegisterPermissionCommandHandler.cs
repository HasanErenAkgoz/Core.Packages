using Core.Packages.Application.Features.Auth.Register.Commands;
using Core.Packages.Application.Features.RolePermissions.Create;
using Core.Packages.Application.Features.Roles.Commands.Create;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using MediatR;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Core.Packages.Application.Features.Permissions.Commands.ScanAndRegister
{
    public class ScanAndRegisterPermissionCommandHandler : IRequestHandler<ScanAndRegisterPermissionsCommand, Unit>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMediator _mediator;
        public ScanAndRegisterPermissionCommandHandler(IPermissionRepository permissionRepository, IMediator mediator)
        {
            _permissionRepository = permissionRepository;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ScanAndRegisterPermissionsCommand request, CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var handlerTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Handler") && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .ToList();

            IDataResult<int> createdRole = await _mediator.Send(new CreateRoleCommand { Name = "System Admin"});
            var createdUser = await _mediator.Send(new RegisterCommand
            {
                FirstName = "System",
                LastName = "Admin",
                Email = "SystemAdmin@CorePackages.com",
                Password = "SystemAdmin123.",
                ConfirmPassword = "SystemAdmin123.",
                IdentityNo = "12345678910",
            });
            foreach (var handlerType in handlerTypes)
            {
                var handlerName = handlerType.Name
                    .Replace("Handler", "")
                    .Replace("Command", "")
                    .Replace("Query", "");

                var description = Regex.Replace(handlerName, "(?<=.)([A-Z])", " $1");

                if (!_permissionRepository.Any(p => p.Name == handlerName))
                {
                    var permission = new Core.Packages.Domain.Entities.Permission
                    {
                        Name = handlerName,       
                        Description = description 
                    };

                    await _permissionRepository.AddAsync(permission, cancellationToken);
                    await _mediator.Send(new CreateRolePermissionCommand { PermissionId = permission.Id, RoleId = createdRole.Data });
                }
            }
        
            return Unit.Value;
        }
    }
}
