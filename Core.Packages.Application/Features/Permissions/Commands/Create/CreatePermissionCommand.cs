using Core.Packages.Application.Shared.Result;
using MediatR;

namespace Core.Packages.Application.Features.Permission.Commands.Create
{
    public class CreatePermissionCommand : IRequest<IResult>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
