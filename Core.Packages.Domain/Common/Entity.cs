namespace Core.Packages.Domain.Common;

public abstract class Entity<TId> : IAuditableEntity, ISoftDelete
{
    public TId Id { get; set; } = default!;
    
    public int CreatedById { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int? ModifiedById { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public EntityStatus Status { get; set; } = EntityStatus.Active;
    public DateTime? DeletedDate { get; set; }
    public int? DeletedById { get; set; }

    protected Entity()
    {
        Id = default!;
        CreatedDate = DateTime.UtcNow;
        Status = EntityStatus.Active;
    }

    protected Entity(TId id)
    {
        Id = id;
        CreatedDate = DateTime.UtcNow;
        Status = EntityStatus.Active;
    }
}