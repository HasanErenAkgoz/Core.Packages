using Core.Packages.Application.Shared.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Features.Auth.Register.Commands
{
    public class RegisterCommand : IRequest<IDataResult<int>>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string IdentityNo { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; } 
        public string ConfirmPassword { get; set; } 
    }
 
}
