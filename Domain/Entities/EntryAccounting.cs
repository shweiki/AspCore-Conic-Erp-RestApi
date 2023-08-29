#nullable disable

namespace Domain.Entities;

public partial class EntryAccounting
{
    public long Id { get; set; }
    public string Description { get; set; }
    public DateTime FakeDate { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }

    public virtual ICollection<EntryMovement> EntryMovements { get; set; }
}
