using Core.Packages.Application.Features.Permissions.DTOs;
using Core.Packages.Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Features.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommand : IRequest<IResult>
    {
        public CreatePermissionDTO Permission { get; set; }
    }
}
