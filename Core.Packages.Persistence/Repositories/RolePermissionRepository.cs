using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.UnitOfWork;
using Core.Packages.Persistence.Context;
using Core.Packages.Persistence.Repositories.EntitiyFrameworkCore;

namespace Core.Packages.Persistence.Repositories
{
    public class RolePermissionRepository : EfEntityRepository<RolePermission, BaseDbContext>, IRolePermissionRepository
    {
        public RolePermissionRepository(BaseDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
