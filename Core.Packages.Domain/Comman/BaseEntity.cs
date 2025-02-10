using Core.Packages.Domain.Enums;

namespace Core.Packages.Domain.Comman
{
    public abstract class BaseEntity<TId> where TId : struct
    {
        public TId Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public TId CreatedBy { get; set; }
        public TId? ModifiedBy { get; set; }
        public Status Status { get; set; }
    }
}
