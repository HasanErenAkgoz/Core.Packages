using Core.Packages.Application.Common.Attributies;
using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.Permission.Queries.GetAll
{
    [Cache("permissions_{UserId}", 30)]
    public class GetAllPermissionQuery : IRequest<IDataResult<IEnumerable<GetPermissionResponse>>>
    {

    }
}
