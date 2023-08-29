using System.ComponentModel;

#nullable disable

namespace Domain.Entities;

public partial class TreeAccount
{
    public long Id { get; set; }
    public string Description { get; set; }
    public int Status { get; set; }
    [DefaultValue("")]
    public string Code { get; set; }
    public string Type { get; set; }
    public bool IsPrime { get; set; }
    public string Name { get; set; }
    public string ParentId { get; set; }

    public virtual ICollection<Bank> Banks { get; set; }
    public virtual ICollection<Cash> Cashes { get; set; }
    public virtual ICollection<EntryMovement> EntryMovements { get; set; }
    public virtual ICollection<Member> Members { get; set; }
    public virtual ICollection<Vendor> Vendors { get; set; }
}
