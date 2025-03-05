using Core.Packages.Application.Common.Services.Auth;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Packages.Application.Features.Roles.Commands.Create
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, IDataResult<int>>
    {
        private readonly RoleManager<Role> _roleManager;
        public CreateRoleCommandHandler(RoleManager<Role> roleManager, IAuthenticationService authenticationService)
        {
            _roleManager = roleManager;
        }

        public async Task<IDataResult<int>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var checkRoleExists = await _roleManager.RoleExistsAsync(request.Name);
                if (!checkRoleExists)
                {
                    Role role = new Role { Name = request.Name };
                    await _roleManager.CreateAsync( role);
                    return new SuccessDataResult<int>(role.Id,"Role created successfully");
                }
                return new ErrorDataResult<int>(1,"Role already exists");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<int>(ex.Message);
            }
           
        }
    }
}
