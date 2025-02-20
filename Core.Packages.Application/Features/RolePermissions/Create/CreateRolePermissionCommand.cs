using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.RolePermissions.Create
{
    public class CreateRolePermissionCommand : IRequest<IResult>
    {
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
    }
}
