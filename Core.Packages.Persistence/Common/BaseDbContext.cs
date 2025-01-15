using Core.Packages.Application.Services;
using Core.Packages.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Packages.Persistence.Common;

public abstract class BaseDbContext : DbContext
{
    protected readonly ICurrentUserService CurrentUserService;

    protected BaseDbContext(DbContextOptions options, ICurrentUserService currentUserService) : base(options)
    {
        CurrentUserService = currentUserService;
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity<int>>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Status = EntityStatus.Active;
                    entry.Entity.CreatedById = CurrentUserService.Id;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedById = CurrentUserService.Id;
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedById = CurrentUserService.Id;
                    entry.Entity.DeletedDate = DateTime.UtcNow;
                    entry.Entity.Status = EntityStatus.Deleted;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Her entity için soft delete filter uygula
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Entity<int>).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var deletedCheck = Expression.Lambda(
                    Expression.NotEqual(
                        Expression.Property(parameter, nameof(Entity<int>.Status)),
                        Expression.Constant(EntityStatus.Deleted)
                    ),
                    parameter
                );
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(deletedCheck);
            }
        }

        base.OnModelCreating(modelBuilder);
    }
} 