using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Application.Common.Services.Auth
{
    public interface IAuthenticationService
    {
       public bool HasPermission(string permission);
       public void EnsurePermissionForHandler<THandler>();
    }
}