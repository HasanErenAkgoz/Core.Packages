using AutoMapper;
using Core.Packages.Application.Common.Attributies;
using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using MediatR;

namespace Core.Packages.Application.Features.Permission.Queries.GetAll
{
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, IDataResult<IEnumerable<GetPermissionResponse>>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper; // 🔹 AutoMapper ekliyoruz!

        public GetAllPermissionQueryHandler(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<IEnumerable<GetPermissionResponse>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var permissions = await _permissionRepository.GetListAsync(cancellationToken);

                var permissionResponses = _mapper.Map<IEnumerable<GetPermissionResponse>>(permissions);

                return new SuccessDataResult<IEnumerable<GetPermissionResponse>>(permissionResponses);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<IEnumerable<GetPermissionResponse>>(ex.Message);
            }
        }
    }

}
