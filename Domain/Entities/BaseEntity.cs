public class BaseEntity<TId>
{
    public TId Id { get; set; }
    public int? CreatedUserId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int? LastUpdatedUserId { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? DeletedUserId { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; } 
}