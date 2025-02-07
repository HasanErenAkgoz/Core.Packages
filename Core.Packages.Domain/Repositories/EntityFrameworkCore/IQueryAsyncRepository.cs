using System.Linq.Expressions;

namespace Core.Packages.Domain.Repositories.EntityFrameworkCore
{
    public interface IQueryAsyncRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken,Expression<Func<T, bool>> expression = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
        TResult InTransaction<TResult>(CancellationToken cancellationToken, Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null);
        Task<int> GetCountAsync(CancellationToken cancellationToken,Expression<Func<T, bool>> expression = null);
    }
}
