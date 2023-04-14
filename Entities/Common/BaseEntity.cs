using System;

namespace Domain.Common;
public abstract class BaseEntity
{
    public long Id { get; set; }
    public Guid UniqueId { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}
