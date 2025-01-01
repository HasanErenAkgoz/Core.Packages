namespace Core.Packages.Domain.Common;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
} 