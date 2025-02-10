using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Persistence.Repositories.EntitiyFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Persistence.Repositories
{
    public class PermissionRepositoriy : EfEntityRepository<Permission, DbContext>, IPermissionRepository
    {
        public PermissionRepositoriy(DbContext context) : base(context)
        {
        }
    }
}
