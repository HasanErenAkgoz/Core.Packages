using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using MediatR;

namespace Core.Packages.Application.Features.RolePermissions.Create
{
    public class CreateRolePermissionCommandHandler : IRequestHandler<CreateRolePermissionCommand, IResult>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public CreateRolePermissionCommandHandler(IRolePermissionRepository rolePermissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<IResult> Handle(CreateRolePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!_rolePermissionRepository.Any(rp => rp.RoleId == request.RoleId && rp.PermissionId == request.PermissionId))
                {
                    await _rolePermissionRepository.AddAsync(new RolePermission { RoleId = request.RoleId, PermissionId = request.PermissionId }, cancellationToken);
                    return new SuccessResult("Role Permission created successfully");
                }
                return new ErrorResult("Role Permission already exists");
            }
            catch (Exception ex) 
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}
