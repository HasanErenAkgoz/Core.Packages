using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Domain.Repositories.NewFolder
{
    public interface IEntityRepository<T> where T : class
    {
        Task<T> AddAsync(T entity,CancellationToken cancellationToken);
        Task<List<T>> BulkAddAsync(List<T> entities);
        T Update(T entity);
        Task<List<T>> BulkUpdateAsync(List<T> entities);
        T Delete(T entity);
        Task<List<T>> BulkDeleteAsync(List<T> entities);
        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken, Expression<Func<T, bool>>? expression = null);
        bool Any(Expression<Func<T, bool>> expression);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<int> GetCountAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> expression = null);
        Task<int> SaveChangesAsync();
        IQueryable<T> Query();

    }
}
