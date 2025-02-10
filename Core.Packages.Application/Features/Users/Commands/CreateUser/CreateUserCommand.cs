using Core.Packages.Application.Features.Users.DTOs;
using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<IResult>
    {
        public CreateUserDTO User { get; set; }
    }
}
