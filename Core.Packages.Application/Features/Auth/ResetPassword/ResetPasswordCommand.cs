﻿using Core.Packages.Application.Shared.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Features.Auth.ResetPassword
{
    public class ResetPasswordCommand : IRequest<IResult>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
