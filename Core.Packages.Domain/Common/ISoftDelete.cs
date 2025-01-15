namespace Core.Packages.Domain.Common;

public interface ISoftDelete
{
    DateTime? DeletedDate { get; set; }
    int? DeletedById { get; set; }
} 