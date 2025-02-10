
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.Result;
using Core.Packages.Domain.UnitOfWork;
using MediatR;

namespace Core.Packages.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, IResult>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                Permission permission = new Permission
                {
                    Name = request.Permission.Name,
                    Description = request.Permission.Description
                };
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return new SuccessResult();

            }
            catch (Exception)
            {

                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return new ErrorResult("An error occurred while creating the permission.");
            }
        }
    }
}
