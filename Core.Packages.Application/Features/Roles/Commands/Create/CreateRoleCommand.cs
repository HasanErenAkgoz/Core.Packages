using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.Roles.Commands.Create
{
    public class CreateRoleCommand : IRequest<IDataResult<int>>
    {
        public string Name { get; set; }
    }
}
