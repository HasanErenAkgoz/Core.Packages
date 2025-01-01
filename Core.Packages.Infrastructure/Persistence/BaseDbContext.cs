using System.Linq.Expressions;
using Core.Packages.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Packages.Infrastructure.Persistence;

public abstract class BaseDbContext : DbContext
{
    public BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Soft delete filter
        foreach (var type in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity<int>).IsAssignableFrom(type.ClrType))
            {
                modelBuilder.Entity(type.ClrType).Property<bool>("IsDeleted");
                var parameter = Expression.Parameter(type.ClrType, "p");
                var deletedCheck = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, "IsDeleted"),
                        Expression.Constant(false)
                    ),
                    parameter
                );
                modelBuilder.Entity(type.ClrType).HasQueryFilter(deletedCheck);
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity<int>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
} 