using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Packages.Domain.Repositories.NewFolder
{
    public interface ICommandAsyncRepository<T> where T : class
    {
        Task<T> AddAsync(T entity,CancellationToken cancellationToken);
        Task<List<T>> BulkAddAsync(List<T> entities);
        T Update(T entity);
        Task<List<T>> BulkUpdateAsync(List<T> entities);
        T Delete(T entity);
        Task<List<T>> BulkDeleteAsync(List<T> entities);


    }
}
