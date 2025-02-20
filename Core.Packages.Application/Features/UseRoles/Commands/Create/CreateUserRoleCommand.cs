using Core.Packages.Application.Shared.Result;
using Core.Packages.Domain.Entities;
using MediatR;

namespace Core.Packages.Application.Features.UseRoles.Commands.Create
{
    public class CreateUserRoleCommand : IRequest<IResult>
    {
        public User User { get; set; }
        public string RoleName { get; set; }
    }
}
