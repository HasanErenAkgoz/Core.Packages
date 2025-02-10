using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Domain.Repositories.EntityFrameworkCore
{
    public interface IUserRepository : IEntityRepository<User>
    {
    }
}
