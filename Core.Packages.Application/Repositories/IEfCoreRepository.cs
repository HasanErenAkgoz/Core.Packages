using System.Linq.Expressions;
using Core.Packages.Domain.Common;
using Core.Utilities.Results;

namespace Core.Packages.Application.Repositories;

public interface IEfCoreRepository<T>
    where T : class{
    Task<T> AddAsync(T entity);
    Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
    T Update(T entity);
    Task DeleteAsync(T entity);
    IEnumerable<T> GetList(Expression<Func<T, bool>> expression = null);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression = null);
    T Get(Expression<Func<T, bool>> expression);
    Task<T> GetAsync(Expression<Func<T, bool>> expression);
    int SaveChanges();
    Task<int> SaveChangesAsync();
    IQueryable<T> Query();
    Task<int> Execute(FormattableString interpolatedQueryString);

    TResult InTransaction<TResult>(Func<TResult> action, Action successAction = null, Action<Exception> exceptionAction = null);

    Task<int> GetCountAsync(Expression<Func<T, bool>> expression = null);
    int GetCount(Expression<Func<T, bool>> expression = null);
}