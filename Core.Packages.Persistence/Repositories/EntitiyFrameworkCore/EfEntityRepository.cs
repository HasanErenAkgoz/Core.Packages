using Core.Packages.Domain.Repositories.NewFolder;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Packages.Persistence.Repositories.EntitiyFrameworkCore
{
    public class EfEntityRepository<TEntity, TContext> : IEntityRepository<TEntity> where TEntity : class where TContext : DbContext
    {
        protected TContext Context { get; }
        public EfEntityRepository(TContext context)
        {
            Context = context;

        }
        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Context.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<List<TEntity>> BulkAddAsync(List<TEntity> entities)
        {
            await Context.BulkInsertAsync(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            Context.Update(entity);
            return entity;
        }

        public async Task<List<TEntity>> BulkUpdateAsync(List<TEntity> entities)
        {
            await Context.BulkUpdateAsync(entities);
            return entities;
        }

        public TEntity Delete(TEntity entity)
        {
            return Context.Remove(entity).Entity;
        }

        public async Task<List<TEntity>> BulkDeleteAsync(List<TEntity> entities)
        {
            await Context.BulkDeleteAsync(entities);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default,Expression < Func<TEntity, bool>> expression = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            
            if (expression != null)
                query = query.Where(expression);
            
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(expression, cancellationToken);
        }
        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default,Expression < Func<TEntity, bool>> expression = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();
            
            if (expression != null)
                query = query.Where(expression);
            
            return await query.CountAsync(cancellationToken);
        }

    }
}
