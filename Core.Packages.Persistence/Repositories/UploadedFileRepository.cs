using Core.Packages.Domain.Entities;
using Core.Packages.Domain.Repositories.EntityFrameworkCore;
using Core.Packages.Domain.UnitOfWork;
using Core.Packages.Persistence.Context;
using Core.Packages.Persistence.Repositories.EntitiyFrameworkCore;

namespace Core.Packages.Persistence.Repositories
{
    public class UploadedFileRepository : EfEntityRepository<UploadedFile, BaseDbContext>, IUploadedFileRepository
    {
        public UploadedFileRepository(BaseDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
