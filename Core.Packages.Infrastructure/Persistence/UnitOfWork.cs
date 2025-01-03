using Core.Packages.Application.Interfaces;
using Core.Packages.Application.Repositories;
using Core.Packages.Domain.Common;
using Core.Packages.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly Dictionary<Type, object> _repositories;
    private bool _disposed;

    public UnitOfWork(DbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IEfCoreRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
    {
        var type = typeof(TEntity);
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = Activator.CreateInstance(
                typeof(EfCoreRepositoryBase<,>).MakeGenericType(type, _context.GetType()), _context);
            _repositories[type] = repositoryInstance!;
        }

        return (IEfCoreRepository<TEntity>)_repositories[type];
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task AddRangeAsync<T>(IEnumerable<T> entities) where T : class
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
            foreach (var repository in _repositories.Values)
            {
                if (repository is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
        _disposed = true;
    }
} 