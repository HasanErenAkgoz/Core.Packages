using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using MediatR;

namespace Core.Packages.Application.Features.Permission.Queries.GetAll
{
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, IDataResult<IEnumerable<GetPermissionResponse>>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetAllPermissionQueryHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        public async Task<IDataResult<IEnumerable<GetPermissionResponse>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return new SuccessDataResult<IEnumerable<GetPermissionResponse>>(await _permissionRepository.GetListAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<IEnumerable<GetPermissionResponse>>(ex.Message);
            }
        }
    }
}
