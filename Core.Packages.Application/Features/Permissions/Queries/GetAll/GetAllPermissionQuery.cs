using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.Permission.Queries.GetAll
{
    public class GetAllPermissionQuery : IRequest<IDataResult<IEnumerable<GetPermissionResponse>>>
    {

    }
}
