using Core.Packages.Application.Repositories;
using Core.Packages.Domain.Common;

namespace Core.Packages.Application.Interfaces;
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    IEfCoreRepository<T> Repository<T>() where T : class, IEntity;
    Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class;
}