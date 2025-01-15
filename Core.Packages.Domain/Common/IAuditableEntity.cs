namespace Core.Packages.Domain.Common;

public interface IAuditableEntity
{
    int CreatedById { get; set; }
    DateTime CreatedDate { get; set; }
    int? ModifiedById { get; set; }
    DateTime? ModifiedDate { get; set; }
} 