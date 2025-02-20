using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using MediatR;

namespace Core.Packages.Application.Features.Permission.Commands.Create
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, IResult>
    {
        private readonly IPermissionRepository _permissionRepository;

        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IResult> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                if (CheckPermissionExist(request.Name))
                {
                    return new ErrorResult("Permission already exists");
                }

                Core.Packages.Domain.Entities.Permission permission = new Core.Packages.Domain.Entities.Permission
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await _permissionRepository.AddAsync(permission, cancellationToken);
                return new SuccessResult("Permission created successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
                throw;
            }
        }
        private bool CheckPermissionExist(string name)
        {
            return _permissionRepository.Any(p => p.Name == name);
        }
    }
}
